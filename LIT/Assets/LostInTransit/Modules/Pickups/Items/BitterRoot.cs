using Moonstorm;
using RoR2;
using UnityEngine;
using R2API;
using System;

namespace LostInTransit.Items
{
    public class BitterRoot : ItemBase
    {
        private const string token = "LIT_ITEM_BITTERROOT_DESC";
        public override ItemDef ItemDef { get; set; } = LITAssets.Instance.MainAssetBundle.LoadAsset<ItemDef>("BitterRoot");

        [ConfigurableField(ConfigName = "Extra Maximum Health per Root", ConfigDesc = "Extra percentage of maximum health added per root")]
        [TokenModifier(token, StatTypes.Default)]
        public static float rootIncrease = 4f;

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<BitterRootBehavior>(stack);
        }
        public class BitterRootBehavior : CharacterBody.ItemBehavior, IBodyStatArgModifier
        {
            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.healthMultAdd += (rootIncrease/100) * stack;
            }
        }
    }
}
