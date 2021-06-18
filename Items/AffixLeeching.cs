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
using static LostInTransit.Elites.LeechingElite;

namespace LostInTransit.Equipment
{
    public class AffixLeeching : EquipmentBase<AffixLeeching>
    {
        public override string EquipmentName => "Guttural Whimpers";

        public override string EquipmentLangTokenName => "AffixLeeching";

        public override string EquipmentPickupDesc => "Become an aspect of Leeching.";

        public override string EquipmentFullDescription => "Become an aspect of Leeching.";

        public override string EquipmentLore => "";

        public override float Cooldown => 10f;

        //public override BuffDef PassiveBuffDef => Elites.LeechingElite.instance.eliteBuffDef;

        public override bool EnigmaCompatible => false;
        public override bool IsBoss { get; } = false;
        public override bool IsLunar { get; } = false;
        public override bool AppearsInSinglePlayer { get; } = true;
        public override bool AppearsInMultiPlayer { get; } = true;
        public override bool CanDrop => false;

        public override GameObject EquipmentModel => MainAssets.LoadAsset<GameObject>("Thallium.prefab");

        public override Sprite EquipmentIcon => MainAssets.LoadAsset<Sprite>("thallium.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }

        public override void Hooks()
        {
        }

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateEquipment();
            Hooks();
        }

        public static float healthStolen;
        public static float regenCooldown;

        public void CreateConfig(ConfigFile config)
        {
            healthStolen = config.Bind<float>("Elite: " + "Leeching", "Health Leeched", 2f, "% of health stolen on hit, based on damage done.").Value;
            regenCooldown = config.Bind<float>("Elite: " + "Leeching", "Regen Cooldown", 15f, "Seconds between healing novas.").Value;
        }

        protected override bool ActivateEquipment(EquipmentSlot slot)
        {
            return false;
        }
    }
}
