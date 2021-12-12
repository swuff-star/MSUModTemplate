using Moonstorm;
using RoR2;
using R2API;

namespace LostInTransit.Buffs
{
    [DisabledContent]
    public class DiceAtk : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = LITAssets.Instance.MainAssetBundle.LoadAsset<BuffDef>("DiceAtk");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<DiceAtkBehavior>(stack);
        }

        public class DiceAtkBehavior : CharacterBody.ItemBehavior, IBodyStatArgModifier
        {
            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.attackSpeedMultAdd += (Items.BlessedDice.atkAmount / 100);
            }
        }
    }
}