using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using static LostInTransit.LostInTransitMain;
using UnityEngine;

namespace LostInTransit.Items
{
    class TelescopicSight : ItemBase
    {
        public override string ItemName => "Telescopic Sight";

        public override string ItemLangTokenName => "CRIT_SCOPE";

        public override string ItemPickupDesc => "Crits have a chance to crit";

        public override string ItemFullDescription => "<style=cIsDamage>10%</style> <style=cStack>(+10% per stack)</style> chance for critical hits to do <style=cIsDamage>doubled damage</style>";

        public override string ItemLore => "pew";

        public override ItemTier Tier => ItemTier.Tier3;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("Amethyst.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("amethyst.png");

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
        public static float dmgMultiplier;
        public static float addCrit;

        public void CreateConfig(ConfigFile config)
        {
            procChance = config.Bind<float>("Item: " + ItemName, "Base Proc Chance", 10f, "Base chance of double critting.").Value;
            stackChance = config.Bind<float>("Item: " + ItemName, "Stacking Proc Chance", 10f, "Added chance to double crit per stack.").Value;
            dmgMultiplier = config.Bind<float>("Item: " + ItemName, "Damage Multiplier", 2f, "How much damage is multiplied by.").Value;
            addCrit = config.Bind<float>("Item: " + ItemName, "Added Crit Chance", 10f, "How much regular crit chance is given.").Value;
        }

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateItem();
            Hooks();
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            GameObject attacker = damageInfo.attacker;
            if (self && attacker)
            {
                var attackerBody = attacker.GetComponent<CharacterBody>();
                int scopeCount = GetCount(attackerBody);
                if (scopeCount > 0)
                {
                    if (damageInfo.crit)
                    {
                        Debug.Log("Pre-scope damage: " + damageInfo.damage);
                        if (Util.CheckRoll((procChance + (stackChance * (scopeCount - 1)))))
                        {
                            damageInfo.damage *= dmgMultiplier;
                            Debug.Log("Scope Triggered for total damage of: " + damageInfo.damage);
                        }
                    }
                }
            }
            orig(self, damageInfo, victim);
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            self.crit += addCrit;
        }
    }
}
