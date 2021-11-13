using Moonstorm;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace LostInTransit.Buffs
{
    [DisabledContent]
    public class NuggetRegen : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("NuggetRegen");

        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
            var schmeat = Resources.Load<BuffDef>("buffdefs/MeatRegenBoost");
            BuffDef.iconSprite = schmeat.iconSprite;
            BuffDef.startSfx = schmeat.startSfx;
        }
        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<NuggetRegenBehavior>(stack);
        }

        public class NuggetRegenBehavior : CharacterBody.ItemBehavior, IStatItemBehavior
        {
            public void RecalculateStatsStart()
            {
            }
            public void RecalculateStatsEnd()
            {
                body.regen *= 1f + (stack * Items.MeatNugget.regenMultiplier);
            }
        }
    }
}
