using Moonstorm;
using RoR2;
using R2API;
using RoR2.Items;

namespace LostInTransit.Buffs
{
    [DisabledContent]
    public class DiceMove : BuffBase
    {
        public override BuffDef BuffDef { get; } = LITAssets.Instance.MainAssetBundle.LoadAsset<BuffDef>("DiceMove");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public class DiceMoveBehavior : BaseItemBodyBehavior, IBodyStatArgModifier
        {
            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.moveSpeedMultAdd += (Items.BlessedDice.moveAmount / 100);
            }
        }
    }
}