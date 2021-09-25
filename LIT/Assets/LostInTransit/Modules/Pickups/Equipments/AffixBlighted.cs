﻿using RoR2;
using UnityEngine;
using Moonstorm;
using Moonstorm.Utilities;

namespace LostInTransit.Equipments
{
    public class AffixBlighted : EliteEquipmentBase
    {
        public override MSEliteDef EliteDef { get; set; } = Assets.LITAssets.LoadAsset<MSEliteDef>("Blighted");
        public override EquipmentDef EquipmentDef { get; set; } = Assets.LITAssets.LoadAsset<EquipmentDef>("AffixBlighted");

        public override MSAspectAbilityDataHolder AspectAbilityData { get; set; } = Assets.LITAssets.LoadAsset<MSAspectAbilityDataHolder>("AbilityBlighted");
        public override bool FireAction(EquipmentSlot slot)
        {
            if(MSUtil.IsModInstalled("com.TheMysticSword.AspectAbilities"))
            {
                var component = slot.characterBody.GetComponent<Buffs.AffixBlighted.AffixBlightedBehavior>();
                if(component)
                {
                    component.MasterBehavior.Ability();
                    return true;
                }
            }
            return false;
        }
    }
}
