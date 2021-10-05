using Moonstorm;
using static R2API.DamageAPI;

namespace LostInTransit.DamageTypes
{
    public class Volatile : DamageTypeBase
    {
        public override ModdedDamageType ModdedDamageType { get; set; }

        public static ModdedDamageType volatileDamageType;

        public override void Initialize()
        {
            volatileDamageType = ReserveDamageType();
            ModdedDamageType = volatileDamageType;
        }

        public override ModdedDamageType GetDamageType()
        {
            return volatileDamageType;
        }
    }
}
