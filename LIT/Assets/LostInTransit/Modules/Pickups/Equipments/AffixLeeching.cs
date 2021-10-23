using Moonstorm;
using RoR2;

namespace LostInTransit.Equipments
{
    public class AffixLeeching : EliteEquipmentBase
    {
        public override MSEliteDef EliteDef { get; set; } = Assets.LITAssets.LoadAsset<MSEliteDef>("Leeching");
        public override EquipmentDef EquipmentDef { get; set; } = Assets.LITAssets.LoadAsset<EquipmentDef>("AffixLeeching");
        public override MSAspectAbilityDataHolder AspectAbilityData { get; set; } = Assets.LITAssets.LoadAsset<MSAspectAbilityDataHolder>("AbilityLeeching");

        public override bool FireAction(EquipmentSlot slot)
        {
            if (MSUtil.IsModInstalled("com.TheMysticSword.AspectAbilities"))
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
