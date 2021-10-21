using RoR2;
using Moonstorm;

namespace LostInTransit.Equipments
{
    public class GiganticAmethyst : EquipmentBase
    {
        public override EquipmentDef EquipmentDef { get; set; } = Assets.LITAssets.LoadAsset<EquipmentDef>("GiganticAmethyst");

        /*public override void DescriptionToken()
        {
            LITUtil.AddTokenToLanguage(EquipmentDef.descriptionToken,
                $"<style=cIsUtility>Reset all skill cooldowns</style> on use.",
                LangEnum.en);
        }*/

        public override bool FireAction(EquipmentSlot slot)
        {
            var sloc = slot.characterBody?.skillLocator;
            if((bool)!sloc)
            {
                return false;
            }
            sloc.ApplyAmmoPack();
            return true;
        }
    }
}
