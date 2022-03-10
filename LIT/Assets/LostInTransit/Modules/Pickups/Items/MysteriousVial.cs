using Moonstorm;
using RoR2;
using R2API;

namespace LostInTransit.Items
{
    public class MysteriousVial : ItemBase
    {
        private const string token = "LIT_ITEM_MYSTERIOUSVIAL_DESC";
        public override ItemDef ItemDef { get; set; } = LITAssets.Instance.MainAssetBundle.LoadAsset<ItemDef>("MysteriousVial");

        [ConfigurableField(ConfigName = "Extra Regen Per Vial", ConfigDesc = "Extra Regeneration added per vial.")]
        [TokenModifier(token, StatTypes.Default, 0)]
        public static float vialRegen = 0.8f;

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<MysteriousVialBehavior>(stack);
        }

        public class MysteriousVialBehavior : CharacterBody.ItemBehavior, IBodyStatArgModifier
        {
            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.baseRegenAdd += vialRegen * stack;
            }
        }
    }
}
