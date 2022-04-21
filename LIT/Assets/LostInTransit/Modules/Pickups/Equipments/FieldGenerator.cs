using Moonstorm;
using RoR2;

namespace LostInTransit.Equipments
{
    public class FieldGenerator : EquipmentBase
    {
        public override EquipmentDef EquipmentDef { get; } = LITAssets.Instance.MainAssetBundle.LoadAsset<EquipmentDef>("FieldGenerator");
        public override bool FireAction(EquipmentSlot slot)
        {
            return false;
        }
    }
}
