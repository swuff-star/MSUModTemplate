using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LostInTransit.Modules;

namespace LostInTransit.Equipments
{
    public class GiganticAmethyst : EquipmentBase
    {
        public override EquipmentDef EquipmentDef { get; set; } = Assets.LITAssets.LoadAsset<EquipmentDef>("GiganticAmethyst");

        public override bool FireAction(EquipmentSlot slot)
        {
            if (slot.hasAuthority)
            {
                var sloc = slot.characterBody?.skillLocator;
                if((bool)!sloc)
                {
                    return false;
                }
                sloc.ApplyAmmoPack();
            }
            return true;
        }
    }
}
