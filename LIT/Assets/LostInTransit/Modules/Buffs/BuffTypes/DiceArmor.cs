using Moonstorm;
using RoR2;


namespace LostInTransit.Buffs
{
    [DisabledContent]
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
            public void RecalculateStatsStart() { }
            public void RecalculateStatsEnd()
            {
                body.armor += (Items.BlessedDice.armorAmount);
            }
            public void OnDestroy()
            {
                body.RecalculateStats();
            }
        }
    }
}