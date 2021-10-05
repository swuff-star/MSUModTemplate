using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moonstorm;

namespace LostInTransit.Equipments
{
    public abstract class LITEquipmentBase : EquipmentBase
    {
        public virtual void Config() { }
        public abstract void DescriptionToken();
    }
}
