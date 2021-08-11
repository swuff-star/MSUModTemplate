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

        public override string ItemFullDescription => $"<style=cIsDamage>{Thallium.procChance}%</style> chance to poison for <style=cIsDamage>{Thallium.duration * Thallium.dmgCoefficient * 100}%</style> <style=cStack>(+{Thallium.duration * Thallium.dmgStack * 100}% per stack)</style><style=cIsDamage> of enemy's base damage</style> over <style=cIsUtility>{Thallium.duration}</style> seconds and slow for <style=cIsUtility>{Thallium.slowMultiplier * 100}% movement speed of enemy's base speed<style=cIsUtility>.";


        public override string ItemLore => "You found it embedded atop a small hill, surrounded by dead grass.\n\nDead insects.\n\nDead lizards.\n\nThe tree it sat beneath had passed long ago.\n\nBrown bark became white dust, drifting away with the wind.\n\nThe dust smelled of decay.\n\nThis scene, to anyone else, would be overwhelming.\n\nAn unholy site, cursed by something unknown.\n\nBut you dug deeper.\n\nYou found something... worse.\n\nA note. An omen. A promise.\n\nBut you were happy it never arrived.\n\nFor her sake.";

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
            On.RoR2.CharacterBody.RecalculateStats += ApplyThalliumSlow;
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
            dmgStack = config.Bind<float>("Item: " + ItemName, "Stacking Damage", 0.625f, "Extra damage added per stack.").Value;
            slowMultiplier = config.Bind<float>("Item: " + ItemName, "Base Slow", 0.75f, "Slow multiplier applied by Thallium, calculates based on enemy base move speed.").Value;
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
                    if (dotController.victimBody) poisonDamage += dotController.victimBody.damage; 
                    dotStack.damage = poisonDamage * damageMultiplier;

                }
            });
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, UnityEngine.GameObject victim)
        {
            GameObject attacker = damageInfo.attacker;
            if (self && attacker)
            {
                var attackerBody = attacker.GetComponent<CharacterBody>();
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
        private void ApplyThalliumSlow(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            if (self.HasBuff(ThalliumBuff))
            {
                self.moveSpeed *= slowMultiplier;
                //Debug.Log("Enemy slowed via Thallium");
            }
        }
    }
}
