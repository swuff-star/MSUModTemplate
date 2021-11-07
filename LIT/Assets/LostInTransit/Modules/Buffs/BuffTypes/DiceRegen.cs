using Moonstorm;
using RoR2;

namespace LostInTransit.Buffs
{
    public class DiceRegen : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("DiceRegen");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<DiceRegenBehavior>(stack);
        }

        public class DiceRegenBehavior : CharacterBody.ItemBehavior, IStatItemBehavior
        {
            private bool statsDirty = false;
            public void RecalculateStatsStart() { }
            public void RecalculateStatsEnd()
            {
                statsDirty = true;
                body.regen += (body.maxHealth * (Items.BlessedDice.healAmount / 100));
            }
            public void OnDestroy()
            {
                if (statsDirty)
                {
                    body.regen -= (body.maxHealth * (Items.BlessedDice.healAmount / 100));
                }
            }
        }
    }
}