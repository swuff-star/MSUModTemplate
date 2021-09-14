using RoR2;
using Moonstorm;
using Moonstorm.Utilities;

namespace LostInTransit.Equipments
{
    [DisabledContent]
    public class AffixFrenzied : EliteEquipmentBase
    {
        public override MSEliteDef EliteDef { get; set; } = Assets.LITAssets.LoadAsset<MSEliteDef>("Frenzied");
        public override EquipmentDef EquipmentDef { get; set; } = Assets.LITAssets.LoadAsset<EquipmentDef>("AffixFrenzied");
        public override MSAspectAbilityDataHolder AspectAbilityData { get; set; } = Assets.LITAssets.LoadAsset<MSAspectAbilityDataHolder>("AbilityFrenzied");
        public override bool FireAction(EquipmentSlot slot)
        {
            if (MSUtil.IsModInstalled("com.TheMysticSword.AspectAbilities"))
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
