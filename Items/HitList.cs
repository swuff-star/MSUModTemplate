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
    /*public class HitList : ItemBase
    {
        public override string ItemName => "The Hit List";

        public override string ItemLangTokenName => "HIT_LIST";

        public override string ItemPickupDesc => "Killing marked enemies increases damage.";

        public override string ItemFullDescription => "Killing marked enemies increases damage.";

        public override string ItemLore => "fhffbfasfoUUFUUBLUFP BUUPB UBP";

        public override ItemTier Tier => ItemTier.Tier3;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("Hit_List.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("HitList.png");
        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateItem();
            Hooks();
        }

        public void CreateConfig(ConfigFile config)
        {
            cooldown = config.Bind<float>("Item: " + ItemName, "Cooldown", 10f, "Time between assigned hits.").Value;
            procDamage = config.Bind<float>("Item: " + ItemName, "Added Damage", 1f, "Added base damage per completed hit.").Value;
            maxDamage = config.Bind<float>("Item: " + ItemName, "Max Damage", 50f, "Cap for added base damage.").Value;
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }

        public override void Hooks()
        {
            GlobalEventManager.onCharacterDeathGlobal += Evt_GEMOnCharacterDeathGlobal;
            On.RoR2.Run.FixedUpdate += On_RunFixedUpdate;
            GetStatCoefficients += Evt_GetStatCoefficients;
        }

       

        public static float cooldown;
        public float procDamage;
        public float maxDamage;

        private Xoroshiro128Plus rng = new Xoroshiro128Plus(0UL);

        private float stopwatch = 0f;
        private void On_RunFixedUpdate(On.RoR2.Run.orig_FixedUpdate orig, Run self)
        {
            orig(self);
            stopwatch -= Time.fixedDeltaTime;
            if (stopwatch > 0f) return;

            stopwatch = cooldown;
            var alive = AliveList();
            int[] totalVsTeam = { 0, 0, 0 };
            int totalVsAll = 0;
            foreach (var cm in alive)
            {
                var icnt = GetCount(cm);
                if (FriendlyFireManager.friendlyFireMode != FriendlyFireManager.FriendlyFireMode.Off)
                {
                    totalVsAll += icnt;
                }
                else
                {
                    if (cm.teamIndex != TeamIndex.Neutral) totalVsTeam[0] += icnt;
                    if (cm.teamIndex != TeamIndex.Player) totalVsTeam[1] += icnt;
                    if (cm.teamIndex != TeamIndex.Monster) totalVsTeam[2] += icnt;
                }
            }
            if (totalVsAll > 0)
            {
                for (var i = 0; i < totalVsAll; i++)
                {
                    if (alive.Count <= 0) break;
                    var next = rng.NextElementUniform(alive);
                    if (next.hasBody)
                        next.GetBody().AddTimedBuff(MarkDebuff, cooldown);
                    else
                        i--;
                    alive.Remove(next);
                }
            }
            else
            {
                List<CharacterMaster>[] aliveTeam = {
                    alive.Where(cm => cm.teamIndex == TeamIndex.Neutral).ToList(),
                    alive.Where(cm => cm.teamIndex == TeamIndex.Player).ToList(),
                    alive.Where(cm => cm.teamIndex == TeamIndex.Monster).ToList()
                };
                for (var list = 0; list <= 2; list++)
                {
                    for (var i = 0; i < totalVsTeam[list]; i++)
                    {
                        if (aliveTeam[list].Count <= 0) break;
                        var next = rng.NextElementUniform(aliveTeam[list]);
                        if (next.hasBody)
                            next.GetBody().AddTimedBuff(MarkDebuff, cooldown);
                        else
                            i--;
                        aliveTeam[list].Remove(next);
                    }
                }
            }
        }
        public ItemDef HitListTally { get; private set; }
        public BuffDef MarkDebuff { get; private set; }
        public BuffDef TallyBuff { get; private set; }

        public static List<CharacterMaster> AliveList(bool playersOnly = false)
        {
            if (playersOnly) return PlayerCharacterMasterController.instances.Where(x => x.isConnected && x.master && x.master.hasBody && x.master.GetBody().healthComponent.alive).Select(x => x.master).ToList();
            else return CharacterMaster.readOnlyInstancesList.Where(x => x.hasBody && x.GetBody().healthComponent.alive).ToList();
        }

        //is it REALLY fair to say "independent of tiler2" when im just gutting it?



        public override void SetupAttributes()
        {
            base.SetupAttributes();
            //I'm 100% sure the reason it's breaking is because this is commented out but I can't figure out how to replace it for the life of me

            MarkDebuff = ScriptableObject.CreateInstance<BuffDef>();
            MarkDebuff.buffColor = Color.yellow;
            MarkDebuff.canStack = false;
            MarkDebuff.isDebuff = true;
            MarkDebuff.name = "HitListDebuff";
            MarkDebuff.iconSprite = MainAssets.LoadAsset<Sprite>("amethyst.png");
            BuffAPI.Add(new CustomBuff(MarkDebuff));

            TallyBuff = ScriptableObject.CreateInstance<BuffDef>();
            TallyBuff.buffColor = Color.yellow;
            TallyBuff.canStack = true;
            TallyBuff.isDebuff = false;
            TallyBuff.name = "HitListBuff";
            TallyBuff.iconSprite = MainAssets.LoadAsset<Sprite>("amethyst.png");
            BuffAPI.Add(new CustomBuff(TallyBuff));

            HitListTally = ScriptableObject.CreateInstance<ItemDef>();
            HitListTally.hidden = true;
            HitListTally.name = "HitListTally";
            HitListTally.tier = ItemTier.NoTier;
            HitListTally.canRemove = false;
            HitListTally.nameToken = "";
            HitListTally.pickupToken = "";
            HitListTally.loreToken = "";
            HitListTally.descriptionToken = "";
            ItemAPI.Add(new CustomItem(HitListTally, new ItemDisplayRuleDict(null)));
        }


        private void Evt_GEMOnCharacterDeathGlobal(DamageReport rep)
        {
            if ((rep.victimBody?.HasBuff(MarkDebuff) ?? false) && GetCount(rep.attackerBody) > 0)
                rep.attackerBody.inventory.GiveItem(HitListTally);
        }
        
        public static event StatHookEventHandler GetStatCoefficients;

        public delegate void StatHookEventHandler(CharacterBody sender, StatHookEventArgs args);

        //Just straight up stealing this from TILER2, am too high to do it better myself.
        public class StatHookEventArgs : EventArgs
        {
            /// <summary>Added to the direct multiplier to base health. MAX_HEALTH ~ (BASE_HEALTH + baseHealthAdd) * (HEALTH_MULT + healthMultAdd).</summary>
            public float healthMultAdd = 0f;
            /// <summary>Added to base health. MAX_HEALTH ~ (BASE_HEALTH + baseHealthAdd) * (HEALTH_MULT + healthMultAdd).</summary>
            public float baseHealthAdd = 0f;
            /// <summary>Added to base shield. MAX_SHIELD ~ BASE_SHIELD + baseShieldAdd.</summary>
            public float baseShieldAdd = 0f;
            /// <summary>Added to the direct multiplier to base health regen. HEALTH_REGEN ~ (BASE_REGEN + baseRegenAdd) * (REGEN_MULT + regenMultAdd).</summary>
            public float regenMultAdd = 0f;
            /// <summary>Added to base health regen. HEALTH_REGEN ~ (BASE_REGEN + baseRegenAdd) * (REGEN_MULT + regenMultAdd).</summary>
            public float baseRegenAdd = 0f;
            /// <summary>Added to base move speed. MOVE_SPEED ~ (BASE_MOVE_SPEED + baseMoveSpeedAdd) * (MOVE_SPEED_MULT + moveSpeedMultAdd)</summary>
            public float baseMoveSpeedAdd = 0f;
            /// <summary>Added to the direct multiplier to move speed. MOVE_SPEED ~ (BASE_MOVE_SPEED + baseMoveSpeedAdd) * (MOVE_SPEED_MULT + moveSpeedMultAdd)</summary>
            public float moveSpeedMultAdd = 0f;
            /// <summary>Added to the direct multiplier to jump power. JUMP_POWER ~ BASE_JUMP_POWER * (JUMP_POWER_MULT + jumpPowerMultAdd)</summary>
            public float jumpPowerMultAdd = 0f;
            /// <summary>Added to the direct multiplier to base damage. DAMAGE ~ (BASE_DAMAGE + baseDamageAdd) * (DAMAGE_MULT + damageMultAdd).</summary>
            public float damageMultAdd = 0f;
            /// <summary>Added to base damage. DAMAGE ~ (BASE_DAMAGE + baseDamageAdd) * (DAMAGE_MULT + damageMultAdd).</summary>
            public float baseDamageAdd = 0f;
            /// <summary>Added to attack speed. ATTACK_SPEED ~ (BASE_ATTACK_SPEED + baseAttackSpeedAdd) * (ATTACK_SPEED_MULT + attackSpeedMultAdd).</summary>
            public float baseAttackSpeedAdd = 0f;
            /// <summary>Added to the direct multiplier to attack speed. ATTACK_SPEED ~ (BASE_ATTACK_SPEED + baseAttackSpeedAdd) * (ATTACK_SPEED_MULT + attackSpeedMultAdd).</summary>
            public float attackSpeedMultAdd = 0f;
            /// <summary>Added to crit chance. CRIT_CHANCE ~ BASE_CRIT_CHANCE + critAdd.</summary>
            public float critAdd = 0f;
            /// <summary>Added to armor. ARMOR ~ BASE_ARMOR + armorAdd.</summary>
            public float armorAdd = 0f;
        }

        private void Evt_GetStatCoefficients(CharacterBody sender, StatHookEventArgs args)
        {
            var add = Mathf.Clamp(procDamage * (sender.inventory?.GetItemCount(HitListTally) ?? 0), 0f, maxDamage);
            args.baseDamageAdd += add;
            sender.SetBuffCount(TallyBuff.buffIndex, Mathf.FloorToInt(add / procDamage));
        }
    }*/
}

