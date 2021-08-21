using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static R2API.DamageAPI;

namespace LostInTransit.DamageTypes
{
    public abstract class DamageTypeBase
    {
        public abstract ModdedDamageType ModdedDamageType { get; set; }

        public abstract ModdedDamageType GetDamageType();

        public virtual void Initialize() { }

        public virtual void Delegates() { }
    }
}
