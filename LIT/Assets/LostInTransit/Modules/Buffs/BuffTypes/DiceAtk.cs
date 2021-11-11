using Moonstorm;
using RoR2;

namespace LostInTransit.Buffs
{
    public class DiceAtk : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("DiceAtk");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<DiceAtkBehavior>(stack);
        }

        public class DiceAtkBehavior : CharacterBody.ItemBehavior, IStatItemBehavior
        {
            public void RecalculateStatsStart() { }
            public void RecalculateStatsEnd()
            {
                body.attackSpeed += (body.baseAttackSpeed + (body.levelAttackSpeed * (body.level - 1))) * (Items.BlessedDice.atkAmount / 100);
                
            }
            public void OnDestroy()
            {
                body.RecalculateStats();
            }
        }
    }
}