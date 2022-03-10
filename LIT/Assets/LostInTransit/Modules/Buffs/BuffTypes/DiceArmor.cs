using Moonstorm;
using RoR2;
using R2API;


namespace LostInTransit.Buffs
{
    [DisabledContent]
    public class DiceArmor : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = LITAssets.Instance.MainAssetBundle.LoadAsset<BuffDef>("DiceArmor");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<DiceArmorBehavior>(stack);
        }

        public class DiceArmorBehavior : CharacterBody.ItemBehavior, IBodyStatArgModifier
        {
            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.armorAdd += Items.BlessedDice.armorAmount;
            }
        }
    }
}