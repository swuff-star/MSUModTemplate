using Moonstorm;
using RoR2;

namespace LostInTransit.Buffs
{
    [DisabledContent]
    public class RepulsionArmorCD : BuffBase
    {
        public override BuffDef BuffDef { get; } = LITAssets.Instance.MainAssetBundle.LoadAsset<BuffDef>("RepulsionArmorCD");
    }
}