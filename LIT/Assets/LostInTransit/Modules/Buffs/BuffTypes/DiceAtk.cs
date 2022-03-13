using Moonstorm;
using RoR2;
using R2API;
using RoR2.Items;

namespace LostInTransit.Buffs
{
    [DisabledContent]
    public class DiceAtk : BuffBase
    {
        public override BuffDef BuffDef { get; } = LITAssets.Instance.MainAssetBundle.LoadAsset<BuffDef>("DiceAtk");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }


        public class DiceAtkBehavior : BaseItemBodyBehavior, IBodyStatArgModifier
        {
            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.attackSpeedMultAdd += (Items.BlessedDice.atkAmount / 100);
            }
        }
    }
}