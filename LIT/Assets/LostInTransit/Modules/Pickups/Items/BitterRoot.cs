using Moonstorm;
using RoR2;
using RoR2.Items;
using UnityEngine;
using R2API;
using System;

namespace LostInTransit.Items
{
    [DisabledContent]
    public class BitterRoot : ItemBase
    {
        private const string token = "LIT_ITEM_BITTERROOT_DESC";
        public override ItemDef ItemDef { get; } = LITAssets.Instance.MainAssetBundle.LoadAsset<ItemDef>("BitterRoot");

        [ConfigurableField(ConfigName = "Extra Maximum Health per Root", ConfigDesc = "Extra percentage of maximum health added per root")]
        [TokenModifier(token, StatTypes.Default)]
        public static float rootIncrease = 4f;

        public class BitterRootBehavior : BaseItemBodyBehavior, IBodyStatArgModifier
        {
            [ItemDefAssociation(useOnClient = true, useOnServer = true, behaviorTypeOverride = typeof(BitterRootBehavior))]
            public static ItemDef GetItemDef() => LITContent.Items.BitterRoot;

            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.healthMultAdd += (rootIncrease/100) * stack;
                
            }
        }
    }
}
