using Moonstorm;
using RoR2;

namespace LostInTransit.Equipments
{
    public class Prescriptions : EquipmentBase
    {
        public override EquipmentDef EquipmentDef { get; } = LITAssets.Instance.MainAssetBundle.LoadAsset<EquipmentDef>("Prescriptions");

        public override bool FireAction(EquipmentSlot slot)
        {
            slot.characterBody.AddTimedBuffAuthority(LITContent.Buffs.Meds.buffIndex, 12);
            return true;                                                 
        }                                                                       
    }                                                                           
                                                                                
}                                                                  
