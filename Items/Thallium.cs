using BepInEx.Configuration;
using R2API;
using R2API.Utils;
using RoR2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityEngine;
using MonoMod.Cil;
using static LostInTransit.LostInTransitMain;
using static RoR2.DotController;
using UnityEngine.Networking;

namespace LostInTransit.Items
{
    public class Thallium : ItemBase
    {
        public override string ItemName => "Thallium";

        public override string ItemLangTokenName => "thallium";

        public override string ItemPickupDesc => "Chance to poison and slow enemies.";

        public override string ItemFullDescription => "Chance to poison for <style=cIsHealth>500% enemy damage%</style> and slow for <style=cIsUtility>50% movement speed<style=cIsUtility> enemies on hit.";

        public override string ItemLore => "stinky.";

        public override ItemTier Tier => ItemTier.Tier3;

        public static BuffDef ThalliumBuff { get; private set; }



        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("Thallium.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("thallium.png");

        public static DotController.DotIndex poisonDot { get; private set; }



        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }

        public override void Hooks()
        {
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        public static float procChance;
        public static float stackChance;
        public static float capChance;
        public static float dmgCoefficient;
        public static float dmgStack;
        public static float slowMultiplier;
        public static int duration;

        public void CreateConfig(ConfigFile config)
        {
            procChance = config.Bind<float>("Item: " + ItemName, "Base Proc Chance", 10f, "Base chance of proc'ing.").Value;
            stackChance = config.Bind<float>("Item: " + ItemName, "Stacking Proc Chance", 0f, "Added chance to proc per stack.").Value;
            capChance = config.Bind<float>("Item: " + ItemName, "Max Proc Chance", 10f, "Max allowed proc chance.").Value;
            dmgCoefficient = config.Bind<float>("Item: " + ItemName, "Base Damage", 1.25f, "Damage coefficient for Thallium, calculates based on enemy base damage.").Value;
            dmgStack = config.Bind<float>("Item: " + ItemName, "Stacking Damage", 2.5f, "Extra damage added per stack.").Value;
            slowMultiplier = config.Bind<float>("Item: " + ItemName, "Base Slow", 0.9f, "Slow multiplier applied by Thallium, calculates based on enemy base move speed.").Value;
            duration = config.Bind<int>("Item: " + ItemName, "Duration", 4, "Duration of the Thallium debuff.").Value;
        }

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateItem();
            CreateBuff();
            Hooks();
        }



        //static internal BuffDef ThalliumBuff;



        private void CreateBuff()
        {
            ThalliumBuff = ScriptableObject.CreateInstance<BuffDef>();
            ThalliumBuff.name = "Thallium Poisoning";
            //ThalliumBuff.buffColor = Color.blue;
            ThalliumBuff.canStack = false;
            ThalliumBuff.isDebuff = true;
            ThalliumBuff.iconSprite = MainAssets.LoadAsset<Sprite>("Thallium.png");



            CustomBuff thalliumBuff = new CustomBuff(ThalliumBuff);

            BuffAPI.Add(new CustomBuff(ThalliumBuff));

            DotController.DotDef thalliumDotDef = new DotController.DotDef
            {
                interval = 0.5f,
                damageCoefficient = 1,
                damageColorIndex = DamageColorIndex.DeathMark,
                associatedBuff = ThalliumBuff
            };
            poisonDot = DotAPI.RegisterDotDef(thalliumDotDef, (dotController, dotStack) =>
            {
                CharacterBody attackerBody = dotStack.attackerObject.GetComponent<CharacterBody>();
                if (attackerBody)
                {
                    float damageMultiplier = dmgCoefficient + dmgStack * (GetCount(attackerBody) - 1);
                    float poisonDamage = 0f;
                    if (dotController.victimBody) poisonDamage += dotController.victimBody.damage; dotStack.damage = poisonDamage * damageMultiplier;

                }
            });
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, UnityEngine.GameObject victim)
        {
            GameObject attacker = damageInfo.attacker;
            if (self && attacker)
            {
                var attackerBody = attacker.GetComponent<CharacterBody>();
                var victimBody = victim.GetComponent<CharacterBody>();
                int thalCount = GetCount(attackerBody);
                if (thalCount > 0)
                {
                    bool flag = (damageInfo.damageType & DamageType.PoisonOnHit) > DamageType.Generic;
                    if ((thalCount > 0 || flag) && (flag || Util.CheckRoll((procChance + (stackChance * (thalCount - 1))))))
                    {
                        ProcChainMask procChainMask = damageInfo.procChainMask;
                        procChainMask.AddProc(ProcType.BleedOnHit);
                        var dotInfo = new InflictDotInfo()
                        {
                            attackerObject = attacker,
                            victimObject = victim,
                            dotIndex = poisonDot,
                            duration = duration,
                            damageMultiplier = dmgCoefficient
                        };
                        DotController.InflictDot(ref dotInfo);
                    }
                }
            }
            orig(self, damageInfo, victim);
        }
        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            if (self.HasBuff(ThalliumBuff))
            {
                self.moveSpeed = ((float)((self.baseMoveSpeed + (self.levelMoveSpeed * (self.level - 1))) * 0.1));
            }
        }
    }
}
