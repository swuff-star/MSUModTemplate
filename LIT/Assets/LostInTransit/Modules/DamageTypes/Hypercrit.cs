using Moonstorm;
using static R2API.DamageAPI;

namespace LostInTransit.DamageTypes
{
    public sealed class Hypercrit : DamageTypeBase
    {
        public override ModdedDamageType ModdedDamageType { get; protected set; }

        public override void Delegates()
        { }
    }
}
