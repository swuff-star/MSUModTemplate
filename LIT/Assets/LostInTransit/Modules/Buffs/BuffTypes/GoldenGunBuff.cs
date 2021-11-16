using Moonstorm;
using RoR2;
using R2API;

namespace LostInTransit.Buffs
{
    public class GoldenGunBuff : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("GoldenGun");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<GoldenGunBuffBehavior>(stack);
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