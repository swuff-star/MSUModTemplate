using RoR2;
using Moonstorm;

namespace LostInTransit.Items
{
    public class MysteriousVial : LITItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("MysteriousVial");

        public static float vialRegen;
        public override void Initialize()
        {
            Config();
            DescriptionToken();
            vialRegen = LITMain.config.Bind<float>("Item: " + ItemDef.name, "Extra Regen Per Vial", 0.8f, "Extra health regeneration adder per vial.").Value;
        }

        public override void Config()
        {
            var section = $"Item: {ItemDef.name}";
            vialRegen = LITMain.config.Bind<float>(section, "Extra Regen Per Vial", 0.8f, "Extra Regeneration added per vial.").Value;
        }

        public override void DescriptionToken()
        {
            LITUtil.AddTokenToLanguage(ItemDef.descriptionToken,
                $"Regenerate an extra <style=cIsHealing>+{vialRegen}</style> <style=cStack>(+{vialRegen} per stack)</style> <style=cIsHealing>hp</style> per second.",
                LangEnum.en);
        }
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
