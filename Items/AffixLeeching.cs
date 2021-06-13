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
using LostInTransit.Equipment;

namespace LostInTransit.Items
{
    public class AffixLeeching : EquipmentBase
    {
        public override string EquipmentName => "Guttural Whimpers";

        public override string EquipmentLangTokenName => "AFFIX_LEECHING";

        public override string EquipmentPickupDesc => "Become an aspect of Leeching.";

        public override string EquipmentFullDescription => "Become an aspect of Leeching.";

        public override string EquipmentLore => "";

        public override float Cooldown => 10;

        //public override BuffDef PassiveBuffDef => Elites.LeechingElite.instance.eliteBuffDef;

        public override bool CanDrop => false;

        public override GameObject EquipmentModel => throw new NotImplementedException();

        public override Sprite EquipmentIcon => throw new NotImplementedException();

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }

        public override void Hooks()
        {

        }

        public override void Init(ConfigFile config)
        {
        }

        protected override bool ActivateEquipment(EquipmentSlot slot)
        {
            return false;
        }
    }
}
