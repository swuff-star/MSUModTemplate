using Moonstorm;
using R2API;
using RoR2;
using R2API;

namespace LostInTransit.Buffs
{
    public class ThalliumPoison : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("ThalliumPoison");
        public static BuffDef buff;
        public static DotController.DotIndex index;

        public override void Initialize()
        {
            buff = BuffDef;
            index = DotAPI.RegisterDotDef(1f, 1f, DamageColorIndex.DeathMark, BuffDef);
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<ThalDebuffBehavior>(stack);
        }

        public class ThalDebuffBehavior : CharacterBody.ItemBehavior, IBodyStatArgModifier
        {
            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.moveSpeedReductionMultAdd += Items.Thallium.slowMultiplier;
            }
        }
    }
}