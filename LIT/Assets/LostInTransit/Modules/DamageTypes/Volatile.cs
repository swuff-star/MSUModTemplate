using Moonstorm;
using static R2API.DamageAPI;

namespace LostInTransit.DamageTypes
{
    public sealed class Volatile : DamageTypeBase
    {
        public override ModdedDamageType ModdedDamageType { get => volatileDamageType; protected set => volatileDamageType = value; }

        public static ModdedDamageType volatileDamageType;

        public override void Delegates()
        {

        }
    }
}
