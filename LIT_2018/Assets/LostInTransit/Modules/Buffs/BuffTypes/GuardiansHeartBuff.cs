using LostInTransit.Items;
using Moonstorm;
using RoR2;
using R2API;

namespace LostInTransit.Buffs
{
    [DisabledContent]
    public class GuardiansHeartBuff : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = LITAssets.Instance.MainAssetBundle.LoadAsset<BuffDef>("GuardiansHeartBuff");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<ShackledDebuffBehavior>(stack);
        }

        public class ShackledDebuffBehavior : CharacterBody.ItemBehavior, IBodyStatArgModifier
        {
            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.armorAdd += GuardiansHeart.heartArmor;
            }
        }
    }
}