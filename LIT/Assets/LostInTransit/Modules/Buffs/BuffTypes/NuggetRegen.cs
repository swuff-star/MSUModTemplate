using Moonstorm;
using RoR2;

namespace LostInTransit.Buffs
{
    public class NuggetRegen : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("LeechingRegen");

        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }
        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<NuggetRegenBehavior>(stack);
        }

        public class NuggetRegenBehavior : CharacterBody.ItemBehavior, IStatItemBehavior
        {
            public void RecalculateStatsStart()
            {
            }
            public void RecalculateStatsEnd()
            {
                body.regen *= 1f + (stack * 0.5f);
            }
        }
    }
}
