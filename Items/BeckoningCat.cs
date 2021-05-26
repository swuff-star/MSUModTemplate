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

        public override string ItemFullDescription => $"Elite monsters have a <style=cIsUtility>{BeckoningCat.baseChance}%</style> <style=cStack>(+{BeckoningCat.stackChance}% per stack)</style> chance to drop items on death. Dropped items have a <style=cIsUtility>{BeckoningCat.baseUnc}%</style> <style=cStack>(+{BeckoningCat.stackUnc}% per stack)</style> chance to be <color=#81d047>Uncommon</color>, and a <style=cIsUtility>{BeckoningCat.baseRare}%</style> <style=cStack>(+{BeckoningCat.stackRare}% per stack)</style> chance to be <color=#f26060>Rare</color>.";

        public override string ItemLore => "Light rain drops fall around an old-timey restaurant. The noise dull, but slowly raising in volume. Their good luck charm sitting there as its paw dip, dip, dipped. \"Hey, Gramps. Don't you think it's about time to close up? No one is going to be visiting like this.\"\n\nThe elderly man simply sits behind the counter, sipping something warm, and musing himself by watching their old plastic cat. Dip, dip, dip. \"No, my boy. It's for this exact reason we should stay open. Someone is on their way.\"\n\nThe young man rolls his eyes and closes the door, as it was now getting too loud to hear. The distant roar of thunder making the old structure creak, the charm rattling on the table. Dip, dip, dip. \"I'm sure they're on their way home at least. Well, we can still tidy up a bit while we wait for this to pass.\"\n\nThe door swings open as an imposing woman strides in, dressed as though she's some commander or government official. As she brushes water off her coat, she scans the menu on the counter with the charm. Dip, dip dip. \"See, my boy? There's always someone who could use a bit of help. And who are we to turn them away? When it rains, it pours after all.\"\n\nThe woman quickly leaves into the storm, and all is quiet, save for the light staccato of raindrops.Soon however, the woman returns, followed by a small platoon of soldiers. Apparently, a night-long celebration for victory at the only half decent store still open. The woman nods to the elderly man as she places her card next to the charm, and its paw dip, dip, dipped.";

        public override ItemTier Tier => ItemTier.Tier2;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("BeckoningCat.prefab");

        public static GameObject ItemDisplayModel => MainAssets.LoadAsset<GameObject>("CatPickup.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("BeckoningCat.png");

        public static float baseChance;
        public static float stackChance;
        public static float capChance;
        public static float baseUnc;
        public static float stackUnc;
        public static float capUnc;
        public static float baseRare;
        public static float stackRare;
        public static float capRare;
        public static float baseEqp;
        public static bool globalStack;
        public static bool inclDeploys;
        //bottom two are uh... i guess temp fixes?
        //private ItemIndex catalogIndex;
        private Xoroshiro128Plus rng = new Xoroshiro128Plus(0UL);
        //fixes, as in, i can build the mod by disabling these.

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateItem();
            Hooks();
        }
        public void CreateConfig(ConfigFile config)
        {
            baseChance = config.Bind<float>("Item: " + ItemName, "Base Drop Chance", 4.5f, "Percent chance for a Beckoning Cat drop to happen at first stack -- as such, multiplicative with Rare/Uncommon chances.").Value;
            stackChance = config.Bind<float>("Item: " + ItemName, "Stacking Drop Chance", 1.5f, "Percent chance for a Beckoning Cat drop to happen per extra stack.").Value;
            capChance = config.Bind<float>("Item: " + ItemName, "Maximum Chance", 100f, "Maximum percent chance for a Beckoning Cat drop on elite kill.").Value;
            baseUnc = config.Bind<float>("Item: " + ItemName, "Chance for Tier 2 Upgrade", 5f, "Percent chance for a Beckoning Cat drop to become Tier 2 at first stack (if it hasn't already become Tier 3)").Value;
            stackUnc = config.Bind<float>("Item: " + ItemName, "Stacking Chance for Tier 2 Upgrade", 1f, "Percent chance for a Beckoning Cat drop to become Tier 2 per extra stack.").Value;
            capUnc = config.Bind<float>("Item: " + ItemName, "Max Chance for Tier 2 Upgrade", 25f, "Maximum percent chance for a Beckoning Cat drop to become Tier 2.").Value;
            baseRare = config.Bind<float>("Item: " + ItemName, "Chance for Tier 3 Upgrade", 0.5f, "Percent chance for a Beckoning Cat drop to become Tier 3 at first stack.").Value;
            stackRare = config.Bind<float>("Item: " + ItemName, "Stacking Chance for Tier 3 Upgrade", 0.25f, "Percent chance for a Becknoning Cat drop to become Tier 3 per extra stack.").Value;
            capRare = config.Bind<float>("Item: " + ItemName, "Max Chance for Tier 3 Upgrade", 5f, "Maximum percent chance for a Beckoning Cat drop to become Tier 3.").Value;
            baseEqp = config.Bind<float>("Item: " + ItemName, "Chance for Equipment", 1f, "Percent chance for a Tier 1 Beckoning Cat drop to become Equipment instead.").Value;
            globalStack = config.Bind<bool>("Item: " + ItemName, "Count Universal Cats", false, "If true, all Beckoning Cats across all living players are counted towards item drops. If false, only the killer's items count. NOTE: This behavior is a remnant of 56 Leaf Clover's behavior in Classic Items, which only exists because all on-kills took effect for all players in Risk of Rain 1.").Value;
            inclDeploys = config.Bind<bool>("Item: " + ItemName, "Count Deployable Cats", false, "If true, deployables (e.g. Engineer turrets) with Beckoning Cat will count towards item drops.").Value;
        }

        public static GameObject ItemBodyModelPrefab;

        public static CharacterModel.RendererInfo[] ItemDisplaySetup(GameObject obj)
        {
            MeshRenderer[] meshes = obj.GetComponentsInChildren<MeshRenderer>();
            CharacterModel.RendererInfo[] renderInfos = new CharacterModel.RendererInfo[meshes.Length];

            for (int i = 0; i < meshes.Length; i++)
            {
                renderInfos[i] = new CharacterModel.RendererInfo
                {
                    defaultMaterial = meshes[i].material,
                    renderer = meshes[i],
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false //We allow the mesh to be affected by overlays like OnFire or PredatoryInstinctsCritOverlay.
                };
            }

            return renderInfos;
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemBodyModelPrefab = ItemDisplayModel;
            var itemDisplay = ItemBodyModelPrefab.AddComponent<ItemDisplay>();
            itemDisplay.rendererInfos = ItemDisplaySetup(ItemBodyModelPrefab);

            ItemDisplayRuleDict rules = new ItemDisplayRuleDict(new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Chest",
                    localPos = new Vector3(0, 0, 0),
                    localAngles = new Vector3(0, 0, 0),
                    localScale = new Vector3(0, 0, 0)
                }
            });
            //Base rules as nothing so I don't have to ever put up with "These displays are too big!" :)

            rules.Add("mdlCommandoDualies", new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
childName = "Chest",
localPos = new Vector3(0.12361F, 0.41574F, -0.18717F),
localAngles = new Vector3(9.22189F, 249.2127F, 325.3237F),
localScale = new Vector3(0.12F, 0.12F, 0.12F)
                }
            });
            rules.Add("mdlHuntress", new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                   childName = "Head",
localPos = new Vector3(-0.00592F, 0.34294F, 0.00836F),
localAngles = new Vector3(2.27749F, 94.87187F, 304.5424F),
localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });
            rules.Add("mdlBandit2", new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Hat",
localPos = new Vector3(0.12758F, 0.05353F, 0.02429F),
localAngles = new Vector3(25.32988F, 169.5921F, 324.7913F),
localScale = new Vector3(0.07F, 0.07F, 0.07F)
                }
            });
            rules.Add("mdlToolbot", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
childName = "Chest",
localPos = new Vector3(2.1336F, 3.04177F, -1.57253F),
localAngles = new Vector3(7.49727F, 234.8763F, 357.7904F),
localScale = new Vector3(0.8F, 0.8F, 0.8F)
                }
});
            rules.Add("mdlEngi", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "CannonHeadR",
localPos = new Vector3(0.13581F, 0.15501F, 0.26817F),
localAngles = new Vector3(315.5076F, 83.6794F, 87.34187F),
localScale = new Vector3(0.12F, 0.12F, 0.12F)
                }
});
            rules.Add("mdlMage", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Chest",
localPos = new Vector3(-0.10056F, 0.38954F, -0.23766F),
localAngles = new Vector3(4.41098F, 270.3525F, 339.8878F),
localScale = new Vector3(0.08F, 0.08F, 0.08F)
                }
});
            rules.Add("mdlMerc", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Chest",
localPos = new Vector3(0.05951F, 0.37595F, -0.21515F),
localAngles = new Vector3(352.9719F, 235.4679F, 353.3822F),
localScale = new Vector3(0.08F, 0.08F, 0.08F)
                }
});
            rules.Add("mdlTreebot", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "FlowerBase",
localPos = new Vector3(0.81839F, 0.81207F, 0.27007F),
localAngles = new Vector3(359.3345F, 161.2868F, 353.739F),
localScale = new Vector3(0.17F, 0.17F, 0.17F)
                }
});
            rules.Add("mdlLoader", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Chest",
localPos = new Vector3(-0.20636F, 0.53658F, 0.20415F),
localAngles = new Vector3(356.0002F, 55.06639F, 359.7709F),
localScale = new Vector3(0.07F, 0.07F, 0.07F)
                }
});
            rules.Add("mdlCroco", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "SpineChest1",
localPos = new Vector3(-0.00452F, 0.37861F, 3.76868F),
localAngles = new Vector3(357.3059F, 92.49225F, 306.8719F),
localScale = new Vector3(0.8F, 0.8F, 0.8F)
                }
});
            rules.Add("mdlCaptain", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Head",
localPos = new Vector3(-0.09996F, 0.17723F, 0.14426F),
localAngles = new Vector3(353.2818F, 66.78971F, 2.15638F),
localScale = new Vector3(0.05F, 0.05F, 0.05F)
                }
});
            return rules;
        }


        public override void Hooks()
        {
            On.RoR2.DeathRewards.OnKilledServer += On_DROnKilledServer;
            
        }


        private static List<CharacterMaster> AliveList(bool playersOnly = false)
        {
            if (playersOnly) return PlayerCharacterMasterController.instances.Where(x => x.isConnected && x.master && x.master.hasBody && x.master.GetBody().healthComponent.alive).Select(x => x.master).ToList();
            else return CharacterMaster.readOnlyInstancesList.Where(x => x.hasBody && x.GetBody().healthComponent.alive).ToList();
        }

        private static void SpawnItemFromBody(CharacterBody src, int tier, Xoroshiro128Plus rng)
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
                    numberofCats += GetCount(chrm);
                }
            else
                numberofCats += GetCount(damageReport.attackerBody);
                //just hope no one disables it

            if (numberofCats < 1) return;

            float rareChance = Math.Min(baseRare + (numberofCats - 1) * stackRare, capRare);
            float uncommonChance = Math.Min(baseUnc + (numberofCats - 1) * stackUnc, capUnc);
            float anyDropChance = Math.Min(baseChance + (numberofCats - 1) * stackChance, capChance);
            //Base drop chance is multiplicative with tier chances -- tier chances are applied to upgrade the dropped item

            if (numberofCats < 1) return;
            if (anyDropChance < 1) return;

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
