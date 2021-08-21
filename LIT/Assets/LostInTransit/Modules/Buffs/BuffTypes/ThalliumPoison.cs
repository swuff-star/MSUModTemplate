using LostInTransit.Modules;
using R2API;
using RoR2;
using System;

namespace LostInTransit.Buffs
{
    public class ThalliumPoison : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("buffThalliumPoison");
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

        public class ThalDebuffBehavior : CharacterBody.ItemBehavior, IStatItemBehavior
        {
            public void RecalcStatsEnd()
            {
                body.moveSpeed *= LostInTransit.Items.Thallium.slowMultiplier;
            }

            public void RecalcStatsStart()
            {
            }
        }
    }
}