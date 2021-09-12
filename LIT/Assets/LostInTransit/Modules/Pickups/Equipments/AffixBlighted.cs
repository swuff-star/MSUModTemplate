using RoR2;
using UnityEngine;
using Moonstorm;
using Moonstorm.Utilities;

namespace LostInTransit.Equipments
{
    [DisabledContent]
    public class AffixBlighted : EliteEquipmentBase
    {
        public override MSEliteDef EliteDef { get; set; } = Assets.LITAssets.LoadAsset<MSEliteDef>("Blighted");
        public override EquipmentDef EquipmentDef { get; set; } = Assets.LITAssets.LoadAsset<EquipmentDef>("AffixBlighted");
        public override AnimationCurveAsset AbilityChance { get; set; }// = Assets.LITAssets.LoadAsset<AnimationCurveAsset>("AbilityBlighted"); //Uncomment this once we're ready for ability support.
        public override float AiMaxUseDistance { get; set; } = 100f;
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
