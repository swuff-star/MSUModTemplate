using Moonstorm;
using RoR2;

namespace LostInTransit.Equipments
{
    public class AffixFrenzied : EliteEquipmentBase
    {
        public override MSEliteDef EliteDef { get; } = LITAssets.Instance.MainAssetBundle.LoadAsset<MSEliteDef>("Frenzied");
        public override EquipmentDef EquipmentDef { get; } = LITAssets.Instance.MainAssetBundle.LoadAsset<EquipmentDef>("AffixFrenzied");
        //public override MSAspectAbilityDataHolder AspectAbilityData { get; } = LITAssets.Instance.MainAssetBundle.LoadAsset<MSAspectAbilityDataHolder>("AbilityFrenzied");
        
        public override bool FireAction(EquipmentSlot slot)
        {
            /*if (MSUtil.IsModInstalled("com.TheMysticSword.AspectAbilities"))
            {
                var component = slot.characterBody.GetComponent<Buffs.AffixFrenzied.AffixFrenziedBehavior>();
                if (component)
                {
                    component.Ability();
                    return true;
                }
            }*/

            return false;
        }
    }
}
