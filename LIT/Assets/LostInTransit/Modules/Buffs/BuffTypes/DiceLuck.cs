using Moonstorm;
using RoR2;

namespace LostInTransit.Buffs
{
    public class DiceLuck : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("DiceLuck");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<DiceLuckBehavior>(stack);
        }

        public class DiceLuckBehavior : CharacterBody.ItemBehavior, IStatItemBehavior
        {
            private bool statsDirty = false;
            public void RecalculateStatsStart() { }
            public void RecalculateStatsEnd()
            {
                if (!statsDirty)
                {
                    statsDirty = true;
                    body.master.luck += (Items.BlessedDice.luckAmount);
                }
            }
            public void OnDestroy()
            {
                if (statsDirty)
                {
                    body.master.luck -= (Items.BlessedDice.luckAmount);
                }
            }
        }
    }
}