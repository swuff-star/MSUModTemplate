using Moonstorm;
using RoR2;
using R2API;

namespace LostInTransit.Buffs
{
    [DisabledContent]
    public class DiceCrit : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("DiceCrit");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<DiceCritBehavior>(stack);
        }

        public class DiceCritBehavior : CharacterBody.ItemBehavior, IBodyStatArgModifier
        {
            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.critAdd += Items.BlessedDice.critAmount;
            }
        }
    }
}