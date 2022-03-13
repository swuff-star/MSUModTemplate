using Moonstorm;
using RoR2;
using R2API;
using RoR2.Items;

namespace LostInTransit.Buffs
{
    [DisabledContent]
    public class DiceCrit : BuffBase
    {
        public override BuffDef BuffDef { get; } = LITAssets.Instance.MainAssetBundle.LoadAsset<BuffDef>("DiceCrit");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }


        public class DiceCritBehavior : BaseItemBodyBehavior, IBodyStatArgModifier
        {
            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.critAdd += Items.BlessedDice.critAmount;
            }
        }
    }
}