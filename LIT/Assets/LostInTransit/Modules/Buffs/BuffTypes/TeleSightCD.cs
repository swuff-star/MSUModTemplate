using Moonstorm;
using RoR2;

namespace LostInTransit.Buffs
{
    public class TeleSightCD : BuffBase
    {
        public override BuffDef BuffDef { get; } = LITAssets.Instance.MainAssetBundle.LoadAsset<BuffDef>("TeleSightCD");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }
    }
}