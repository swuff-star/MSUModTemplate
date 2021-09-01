using LostInTransit.Equipments;
using LostInTransit.ScriptableObjects;
using RoR2;
using LostInTransit.Modules;
using LostInTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspectAbilities;
using UnityEngine;

namespace LostInTransit.Equipments
{
    public class AffixBlighted : EliteEquipment
    {
        public override LITEliteDef EliteDef { get; set; } = Assets.LITAssets.LoadAsset<LITEliteDef>("Blighted");
        public override EquipmentDef EquipmentDef { get; set; } = Assets.LITAssets.LoadAsset<EquipmentDef>("AffixBlighted");
        public override LITAspectAbility AspectAbility { get; set; } = Assets.LITAssets.LoadAsset<LITAspectAbility>("AbilityBlighted");

        public override bool FireAction(EquipmentSlot slot)
        {
            if(LITMain.AspectAbilitiesInstalled)
            {
                Debug.Log("sup? you a fan of aspect abilities? that's pretty rad");
            }
            return false;
        }
    }
}
