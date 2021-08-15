using LostInTransit.Modules;
using RoR2;

namespace LostInTransit.Items
{
    public class MysteriousVial : ItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("MysteriousVial");

        public static float vialRegen;
        public override void Initialize()
        {
            vialRegen = LITMain.config.Bind<float>("Item: " + ItemDef.name, "Extra Regen Per Vial", 0.4f, "Extra health regeneration adder per vial.").Value;
        }
        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<MysteriousVialBehavior>(stack);
        }

        public class MysteriousVialBehavior : CharacterBody.ItemBehavior, IStatItemBehavior
        {
            public void RecalcStatsStart() { }
            public void RecalcStatsEnd()
            {
                body.regen += vialRegen * stack;
            }
        }
    }
}
