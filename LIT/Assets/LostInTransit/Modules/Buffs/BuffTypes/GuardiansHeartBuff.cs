using LostInTransit.Items;
using Moonstorm;
using RoR2;
using R2API;

namespace LostInTransit.Buffs
{
    [DisabledContent]
    public class GuardiansHeartBuff : BuffBase
    {
        public override BuffDef BuffDef { get; } = LITAssets.Instance.MainAssetBundle.LoadAsset<BuffDef>("GuardiansHeartBuff");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
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