using Moonstorm;
using RoR2;
using R2API;

namespace LostInTransit.Buffs
{
    public class GoldenGunBuff : BuffBase
    {
        public override BuffDef BuffDef { get; } = LITAssets.Instance.MainAssetBundle.LoadAsset<BuffDef>("GoldenGun");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public class GoldenGunBuffBehavior : CharacterBody.ItemBehavior, IBodyStatArgModifier
        {
            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.damageMultAdd += 0.01f * stack;
            }
        }
    }
}