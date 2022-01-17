using Moonstorm;
using RoR2;

namespace LostInTransit.Equipments
{
    public class GiganticAmethyst : EquipmentBase
    {
        public override EquipmentDef EquipmentDef { get; set; } = LITAssets.Instance.MainAssetBundle.LoadAsset<EquipmentDef>("GiganticAmethyst");

        public override bool FireAction(EquipmentSlot slot)
        {
            var sloc = slot.characterBody?.skillLocator;
            if ((bool)!sloc)
            {
                return false;
            }
            sloc.ApplyAmmoPack();
            Util.PlaySound("AmethystProc", slot.characterBody.gameObject); //Why the fuck is this called "AmethystProc", theres nothing to proc, wtf swuff
            return true;
        }
    }
}
