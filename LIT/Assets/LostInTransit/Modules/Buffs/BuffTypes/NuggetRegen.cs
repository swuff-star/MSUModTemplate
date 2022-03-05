﻿using Moonstorm;
using RoR2;
using UnityEngine;
using R2API;

namespace LostInTransit.Buffs
{
    [DisabledContent]
    public class NuggetRegen : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = LITAssets.Instance.MainAssetBundle.LoadAsset<BuffDef>("NuggetRegen");

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

        public class NuggetRegenBehavior : CharacterBody.ItemBehavior, IBodyStatArgModifier
        {
            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.regenMultAdd += Items.MeatNugget.regenMultiplier * stack;
            }
        }
    }
}