using LostInTransit.Modules;
using R2API;
using RoR2;
using Moonstorm;
using System;

namespace LostInTransit.Buffs
{
    public class Shackled : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("Shackled");
        public static BuffDef buff;
        public static DotController.DotIndex index;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<ShackledDebuffBehavior>(stack);
        }

        public class ShackledDebuffBehavior : CharacterBody.ItemBehavior, IStatItemBehavior
        {
            public void RecalculateStatsEnd()
            {
                body.attackSpeed *= (1 - Items.PrisonShackles.slowMultiplier);
            }

            public void RecalculateStatsStart()
            {
            }
        }
    }
}