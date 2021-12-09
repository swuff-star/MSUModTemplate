using Moonstorm;
using RoR2;
using R2API;

namespace LostInTransit.Buffs
{
    [DisabledContent]
    public class DiceMove : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("DiceMove");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<DiceMoveBehavior>(stack);
        }

        public class DiceMoveBehavior : CharacterBody.ItemBehavior, IBodyStatArgModifier
        {
            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.moveSpeedMultAdd += (Items.BlessedDice.moveAmount / 100);
            }
        }
    }
}