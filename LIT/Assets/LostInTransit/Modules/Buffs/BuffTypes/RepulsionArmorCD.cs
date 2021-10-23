using Moonstorm;
using RoR2;

namespace LostInTransit.Buffs
{
    public class RepulsionArmorCD : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("RepulsionArmorCD");
        public static BuffDef buff;
        public override void Initialize()
        {
            buff = BuffDef;
        }
    }
}