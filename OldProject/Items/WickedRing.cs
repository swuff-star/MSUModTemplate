using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static LostInTransit.LostInTransitMain;

namespace LostInTransit.Items
{
    /*class WickedRing : ItemBase
    {
        public override string ItemName => "Wicked Ring";

        public override string ItemLangTokenName => "WICKED_RING_LIT";

        public override string ItemPickupDesc => "Critical hits reduce all skill cooldowns.";

        public override string ItemFullDescription => "Gain <style=cIsUtility>10% critical chance</style>. <style=cIsUtility>Critical strikes</style> reduce all skill cooldowns by <style=cIsUtility>1</style> <style=cStack>(+1 per stack)</style> <style=cIsUtility>seconds</style>.";

        public override string ItemLore => "A ring of the late King. The last of many cursed artifacts from a forgotten kingdom.\n\nEmpowered by the very souls of his progeny, he sacrificed the future of his kin for the future of his subjects. The King used the strength of his rings to protect and rule for generations.\n\nThe King had sacrificed much, all for the sake of those who would worship him. Worship, however, is a dangerous resource. A resourced deserved of deities, not of mere mortals. A simple truth he had forgotten.\n\nThe King had believed too much in his own people, his own strength, his own rings. The King had defied death for generations. Much longer than any man should live, much less rule. The King felt he'd overcome death. He'd thought he had ascended to a figure worthy of his people's worship.\n\nAlas, his infinite wisdom gave way to hubris. He thought he could challenge <style=cIsHealth>Her</style>, and allow his kingdom to prosper under his rule for eons to come. But death is something no one can escape, and delivers all to the same end. He was punished with the loss of what he'd sacrificed so much for, before sacrificing himself for one last hope. His soul never escaping the same torment he'd bestowed unto his sons.\n\n~Memento Mori";

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

        public static float cooldownReduction;
        public static float stackingCDR;
        public static float addCrit;

        public void CreateConfig(ConfigFile config)
        {
            cooldownReduction = config.Bind<float>("Item: " + ItemName, "Cooldown Reduction", 1f, "Cooldown reduced when proc'ed.").Value;
            stackingCDR = config.Bind<float>("Item: " + ItemName, "Stacking Cooldown Reduction", 1f, "How many additional seconds are reduced from extra stacks.").Value;
            addCrit = config.Bind<float>("Item: " + ItemName, "Added Crit Chance", 10f, "How much crit chance is given").Value;
        }

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateItem();
            Hooks();
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            self.crit += addCrit;
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            GameObject attacker = damageInfo.attacker;
            if (self && attacker)
            {
                if (damageInfo.crit)
                {
                    var attackerBody = attacker.GetComponent<CharacterBody>();
                    int ringCount = GetCount(attackerBody);
                    if (ringCount > 0)
                    {

                    }
                }
            }
        }
    }*/
}
