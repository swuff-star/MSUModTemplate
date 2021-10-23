using Moonstorm;
using RoR2;

namespace LostInTransit.Items
{
    public class MysteriousVial : ItemBase
    {
        private const string token = "LIT_ITEM_MYSTERIOUSVIAL_DESC";
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("MysteriousVial");

        [ConfigurableField(ConfigName = "Extra Regen Per Vial", ConfigDesc = "Extra Regeneration added per vial.")]
        [TokenModifier(token, StatTypes.Default, 0)]
        public static float vialRegen = 0.8f;

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<MysteriousVialBehavior>(stack);
        }

        public class MysteriousVialBehavior : CharacterBody.ItemBehavior, IStatItemBehavior
        {
            public void RecalculateStatsStart() { }
            public void RecalculateStatsEnd()
            {
                body.regen += vialRegen * stack + ((body.level - 1) * vialRegen * 0.2f);
            }
        }
    }
}
