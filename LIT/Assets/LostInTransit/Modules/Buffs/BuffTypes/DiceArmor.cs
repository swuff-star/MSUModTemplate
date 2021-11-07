using Moonstorm;
using RoR2;


namespace LostInTransit.Buffs
{
    public class DiceArmor : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("DiceArmor");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<DiceArmorBehavior>(stack);
        }

        public class DiceArmorBehavior : CharacterBody.ItemBehavior, IStatItemBehavior
        {
            private bool statsDirty = false;
            public void RecalculateStatsStart() { }
            public void RecalculateStatsEnd()
            {
                statsDirty = true;
                body.armor += (Items.BlessedDice.armorAmount);
            }
            public void OnDestroy()
            {
                if (statsDirty)
                {
                    body.armor -= (Items.BlessedDice.armorAmount);
                }
            }
        }
    }
}