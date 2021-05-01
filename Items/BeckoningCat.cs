using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityEngine;
using MonoMod.Cil;
using static LostInTransit.LostInTransitMain;

namespace LostInTransit.Items
{
    public class BeckoningCat : ItemBase
    {
        public override string ItemName => "Beckoning Cat";

        public override string ItemLangTokenName => "BECKONING_CAT";

        public override string ItemPickupDesc => "Elite monsters have a chance to drop items.";

        public override string ItemFullDescription => "Elite monsters have a chance to drop items.";

        public override string ItemLore => "Light rain drops fall around an old-timey restaurant. The noise dull, but slowly raising in volume. Their good luck charm sitting there as its paw dip, dip, dipped. \"Hey, Gramps. Don't you think it's about time to close up? No one is going to be visiting like this.\"\n\nThe elderly man simply sits behind the counter, sipping something warm, and musing himself by watching their old plastic cat. Dip, dip, dip. \"No, my boy. It's for this exact reason we should stay open. Someone is on their way.\"\n\nThe young man rolls his eyes and closes the door, as it was now getting too loud to hear. The distant roar of thunder making the old structure creak, the charm rattling on the table. Dip, dip, dip. \"I'm sure they're on their way home at least. Well, we can still tidy up a bit while we wait for this to pass.\"\n\nThe door swings open as an imposing woman strides in, dressed as though she's some commander or government official. As she brushes water off her coat, she scans the menu on the counter with the charm. Dip, dip dip. \"See, my boy? There's always someone who could use a bit of help. And who are we to turn them away? When it rains, it pours after all.\"\n\nThe woman quickly leaves into the storm, and all is quiet, save for the light staccato of raindrops.Soon however, the woman returns, followed by a small platoon of soldiers. Apparently, a night-long celebration for victory at the only half decent store still open. The woman nods to the elderly man as she places her card next to the charm, and its paw dip, dip, dipped.";

        public override ItemTier Tier => ItemTier.Tier2;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("stick.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("stick.png");

        public float baseChance;
        public float stackChance;
        public float capChance;
        public float baseUnc;
        public float stackUnc;
        public float capUnc;
        public float baseRare;
        public float stackRare;
        public float capRare;
        public float baseEqp;
        public bool globalStack;
        public bool inclDeploys;




        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateItem();
            Hooks();
        }
        public void CreateConfig(ConfigFile config)
        {
            baseChance = config.Bind<float>("Item: " + ItemName, "Base Drop Chance", 4f, "Percent chance for a Beckoning Cat drop to happen at first stack -- as such, multiplicative with Rare/Uncommon chances.").Value;
            stackChance = config.Bind<float>("Item: " + ItemName, "Stacking Drop Chance", 1.5f, "Percent chance for a Beckoning Cat drop to happen per extra stack.").Value;
            capChance = config.Bind<float>("Item: " + ItemName, "Maximum Chance", 100f, "Maximum percent chance for a Beckoning Cat drop on elite kill.").Value;
            baseUnc = config.Bind<float>("Item: " + ItemName, "Chance for Tier 2 Upgrade", 1f, "Percent chance for a Beckoning Cat drop to become Tier 2 at first stack (if it hasn't already become Tier 3)").Value;
            stackUnc = config.Bind<float>("Item: " + ItemName, "Stacking Chance for Tier 2 Upgrade", 0.1f, "Percent chance for a Beckoning Cat drop to become Tier 2 per extra stack.").Value;
            capUnc = config.Bind<float>("Item: " + ItemName, "Max Chance for Tier 2 Upgrade", 25f, "Maximum percent chance for a Beckoning Cat drop to become Tier 2.").Value;
            baseRare = config.Bind<float>("Item: " + ItemName, "Chance for Tier 3 Upgrade", 0.01f, "Percent chance for a Beckoning Cat drop to become Tier 3 at first stack.").Value;
            stackRare = config.Bind<float>("Item: " + ItemName, "Stacking Chance for Tier 3 Upgrade", 0.001f, "Percent chance for a Becknoning Cat drop to become Tier 3 per extra stack.").Value;
            capRare = config.Bind<float>("Item: " + ItemName, "Max Chance for Tier 3 Upgrade", 1f, "Maximum percent chance for a Beckoning Cat drop to become Tier 3.").Value;
            baseEqp = config.Bind<float>("Item: " + ItemName, "Chance for Equipment", 1f, "Percent chance for a Tier 1 Beckoning Cat drop to become Equipment instead.").Value;
            globalStack = config.Bind<bool>("Item: " + ItemName, "Count Universal Cats", true, "If true, all Beckoning Cats across all living players are counted towards item drops. If false, only the killer's items count.").Value;
            inclDeploys = config.Bind<bool>("Item: " + ItemName, "Count Deployable Cats", false, "If true, deployables (e.g. Engineer turrets) with Beckoning Cat will count towards item drops.").Value;
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }


        public override void Hooks()
        {
        }



        public static List<CharacterMaster> AliveList(bool playersOnly = false)
        {
            if (playersOnly) return PlayerCharacterMasterController.instances.Where(x => x.isConnected && x.master && x.master.hasBody && x.master.GetBody().healthComponent.alive).Select(x => x.master).ToList();
            else return CharacterMaster.readOnlyInstancesList.Where(x => x.hasBody && x.GetBody().healthComponent.alive).ToList();
        }

        public static void SpawnItemFromBody(CharacterBody src, int tier, Xoroshiro128Plus rng)
        {
            List<PickupIndex> spawnList;
            switch (tier)
            {
                case 1:
                    spawnList = Run.instance.availableTier2DropList;
                    break;
                case 2:
                    spawnList = Run.instance.availableTier3DropList;
                    break;
                case 3:
                    spawnList = Run.instance.availableLunarDropList;
                    break;
                case 4:
                    spawnList = Run.instance.availableNormalEquipmentDropList;
                    break;
                case 5:
                    spawnList = Run.instance.availableLunarEquipmentDropList;
                    break;
                case 0:
                    spawnList = Run.instance.availableTier1DropList;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("tier", tier, "spawnItemFromBody: Item tier must be between 0 and 5 inclusive");
            }
            PickupDropletController.CreatePickupDroplet(spawnList[rng.RangeInt(0, spawnList.Count)], src.transform.position, new Vector3(UnityEngine.Random.Range(-5.0f, 5.0f), 20f, UnityEngine.Random.Range(-5.0f, 5.0f)));
        }



        private void On_DROnKilledServer(On.RoR2.DeathRewards.orig_OnKilledServer orig, DeathRewards self, DamageReport damageReport)
        {
            orig(self, damageReport);

            if (damageReport == null) return;
            CharacterBody victimBody = damageReport.victimBody;
            if (victimBody == null || victimBody.teamComponent.teamIndex != TeamIndex.Monster || !victimBody.isElite) return;
            int numberofCats = 0;
            if (globalStack)
                foreach (CharacterMaster chrm in AliveList())
                {
                    if (!inclDeploys && chrm.GetComponent<Deployable>()) continue;
                    numberofCats += chrm?.inventory?.GetItemCount(catalogIndex) ?? 0;
                }
            else
                numberofCats += damageReport.attackerMaster?.inventory?.GetItemCount(catalogIndex) ?? 0;

            if (numberofCats == 0) return;

            float rareChance = Math.Min(baseRare + (numberofCats - 1) * stackRare, capRare);
            float uncommonChance = Math.Min(baseUnc + (numberofCats - 1) * stackUnc, capUnc);
            float anyDropChance = Math.Min(baseChance + (numberofCats - 1) * stackChance, capChance);
            //Base drop chance is multiplicative with tier chances -- tier chances are applied to upgrade the dropped item

            if (Util.CheckRoll(anyDropChance))
            {
                int tier;
                if (Util.CheckRoll(rareChance))
                {
                    tier = 2;
                }
                else if (Util.CheckRoll(uncommonChance))
                {
                    tier = 1;
                }
                else
                {
                    if (Util.CheckRoll(baseEqp))
                        tier = 4;
                    else
                        tier = 0;
                }
                SpawnItemFromBody(victimBody, tier, rng);
            }
        }
    }
}
