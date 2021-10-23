using Moonstorm;
using RoR2;

namespace LostInTransit.Buffs
{
    public class TeleSightCD : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("TeleSightCD");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }
    }
}