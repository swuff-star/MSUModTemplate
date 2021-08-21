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
    public class AffixLeeching : EliteEquipment
    {
        public override LITEliteDef EliteDef { get; set; } = Assets.LITAssets.LoadAsset<LITEliteDef>("Leeching");
        public override EquipmentDef EquipmentDef { get; set; } = Assets.LITAssets.LoadAsset<EquipmentDef>("AffixLeeching");
        public override LITAspectAbility AspectAbility { get; set; } = Assets.LITAssets.LoadAsset<LITAspectAbility>("AbilityLeeching");

        public override bool FireAction(EquipmentSlot slot)
        {
            if(LITMain.AspectAbilitiesInstalled)
            {
                var component = slot.characterBody.GetComponent<Buffs.AffixLeeching.AffixLeechingBehavior>();
                if (component)
                {
                    component.Ability();
                    return true;
                }
            }
            return false;
        }
    }
}
