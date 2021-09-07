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
    public class AffixFrenzied : EliteEquipment
    {
        public override LITEliteDef EliteDef { get; set; } = Assets.LITAssets.LoadAsset<LITEliteDef>("Frenzied");
        public override EquipmentDef EquipmentDef { get; set; } = Assets.LITAssets.LoadAsset<EquipmentDef>("AffixFrenzied");
        public override LITAspectAbility AspectAbility { get; set; } = Assets.LITAssets.LoadAsset<LITAspectAbility>("AbilityFrenzied");

        public override bool FireAction(EquipmentSlot slot)
        {
            if(LITMain.AspectAbilitiesInstalled)
            {
                //var component = slot.characterBody.GetComponent<Buffs.AffixFrenzied.AffixFrenziedBehavior>();
                /*if (component)
                {
                    component.Ability();
                    return true;
                }*/
            }
            return false;
        }
    }
}
