using BepInEx.Configuration;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;



namespace LostInTransit.Elites
{
    class LeechingElite : EliteBase
    {
        public override string EliteName => "Leeching";

        public override EquipmentDef AffixEquip => new Equipment.AffixLeeching();

        public override Color32 EliteColor => throw new NotImplementedException();

        public override string EliteToken => throw new NotImplementedException();

        public override void Init(ConfigFile config)
        {
            throw new NotImplementedException();
        }
    }
}
