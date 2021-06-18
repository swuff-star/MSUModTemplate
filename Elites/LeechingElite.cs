using BepInEx.Configuration;
using LostInTransit.Equipment;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static LostInTransit.LostInTransitMain;


namespace LostInTransit.Elites
{
    class LeechingElite : EliteBase
    {

        public override string EliteName => "Leeching";
        public override EquipmentDef AffixEquip => AffixLeeching.instance.EquipmentDef;
        public override Color32 EliteColor => new Color32(30, 206, 51, 255);
        public override string EliteToken => "Leeching";
        public override Sprite affixIconSprite => MainAssets.LoadAsset<Sprite>("thallium.png");
        public override int desiredTierIndex => 1;

        public static float healthStolen;
        public static float regenCooldown;

        public void CreateConfig(ConfigFile config)
        {
            healthStolen = config.Bind<float>("Elite: " + EliteName, "Health Leeched", 2f, "% of health stolen on hit, based on damage done.").Value;
            regenCooldown = config.Bind<float>("Elite :" + EliteName, "Regen Cooldown", 15f, "Seconds between healing novas.").Value;
        }

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateBuff();
            Hooks();
        }

        public override void Hooks()
        {
             On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void CreateBuff()
        {

        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, UnityEngine.GameObject victim)
        { }
    }
}
