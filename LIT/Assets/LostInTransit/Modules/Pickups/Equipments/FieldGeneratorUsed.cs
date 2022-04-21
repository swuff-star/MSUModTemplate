using Moonstorm;
using RoR2;

namespace LostInTransit.Equipments
{
    public class FieldGeneratorUsed : EquipmentBase
    {
        public override EquipmentDef EquipmentDef { get; } = LITAssets.Instance.MainAssetBundle.LoadAsset<EquipmentDef>("FieldGeneratorUsed");
        public override bool FireAction(EquipmentSlot slot)
        {
            return false;
        }
    }
}
