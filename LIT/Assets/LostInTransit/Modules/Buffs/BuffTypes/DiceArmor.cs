using Moonstorm;
using RoR2;
using R2API;
using RoR2.Items;

namespace LostInTransit.Buffs
{
    [DisabledContent]
    public class DiceArmor : BuffBase
    {
        public override BuffDef BuffDef { get; } = LITAssets.Instance.MainAssetBundle.LoadAsset<BuffDef>("DiceArmor");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }


        public class DiceArmorBehavior : BaseItemBodyBehavior, IBodyStatArgModifier
        {
            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.armorAdd += Items.BlessedDice.armorAmount;
            }
        }
    }
}