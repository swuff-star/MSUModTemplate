using Moonstorm;
using RoR2;

namespace LostInTransit.Buffs
{
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

        public class DiceMoveBehavior : CharacterBody.ItemBehavior, IStatItemBehavior
        {
            private bool statsDirty = false;
            public void RecalculateStatsStart() { }
            public void RecalculateStatsEnd()
            {
                statsDirty = true;
                body.moveSpeed += (body.baseMoveSpeed + (body.levelMoveSpeed * (body.level - 1))) * (Items.BlessedDice.moveAmount / 100);
            }
            public void OnDestroy()
            {
                if (statsDirty)
                {
                    body.moveSpeed -= (body.baseMoveSpeed + (body.levelMoveSpeed * (body.level - 1))) * (Items.BlessedDice.moveAmount / 100);
                }
            }
        }
    }
}