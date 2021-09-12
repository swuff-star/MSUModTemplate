using RoR2;
using Moonstorm;
using Moonstorm.Utilities;

namespace LostInTransit.Equipments
{
    public class AffixLeeching : EliteEquipmentBase
    {
        public override MSEliteDef EliteDef { get; set; } = Assets.LITAssets.LoadAsset<MSEliteDef>("Leeching");
        public override EquipmentDef EquipmentDef { get; set; } = Assets.LITAssets.LoadAsset<EquipmentDef>("AffixLeeching");
        public override AnimationCurveAsset AbilityChance { get; set; } = Assets.LITAssets.LoadAsset<AnimationCurveAsset>("AbilityLeeching");
        public override float AiMaxUseDistance { get; set; } = 50f;

        public override bool FireAction(EquipmentSlot slot)
        {
            if(MSUtil.IsModInstalled("com.TheMysticSword.AspectAbilities"))
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
