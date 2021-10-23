using Moonstorm;
using RoR2;

namespace LostInTransit.Equipments
{
    public class GiganticAmethyst : EquipmentBase
    {
        public override EquipmentDef EquipmentDef { get; set; } = Assets.LITAssets.LoadAsset<EquipmentDef>("GiganticAmethyst");

        public override bool FireAction(EquipmentSlot slot)
        {
            var sloc = slot.characterBody?.skillLocator;
            if ((bool)!sloc)
            {
                return false;
            }
            sloc.ApplyAmmoPack();
            return true;
        }
    }
}
