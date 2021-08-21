using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static R2API.DamageAPI;

namespace LostInTransit.DamageTypes
{
    public class Hypercrit : DamageTypeBase
    {
        public override ModdedDamageType ModdedDamageType { get; set; }

        public static ModdedDamageType hypercritDamageType;

        public override void Initialize()
        {
            hypercritDamageType = ReserveDamageType();
            ModdedDamageType = hypercritDamageType;
        }

        public override ModdedDamageType GetDamageType()
        {
            return hypercritDamageType;
        }
    }
}
