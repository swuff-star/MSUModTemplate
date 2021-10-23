using Moonstorm;
using RoR2;

namespace LostInTransit.Items
{
    public class MysteriousVial : ItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("MysteriousVial");

        ///To properly use a ConfigurableField attribute, you need to put it on top of a field that has these 2 properties:
        ///> Is Static.
        ///> Is Public.
        ///If you need to replace the placeholders, you need to use the following properties.
        ///
        ///ConfigSection: Custom string for defining the Section, not reccomended to use it on items/equipments for reasons i wont go on here.
        ///ConfigName: Custom string for defining the name of the config.
        ///Config Desc: Custom string for defining the description of the config.
        [ConfigurableField(ConfigName = "Extra Regen Per Vial", ConfigDesc = "Extra Regeneration added per vial.")]
        public static float vialRegen = 0.8f;

        //DELETE ME AFTER MIGRATING CONFIGS TO ATTRIBUTES.
        /*public override void Config()
        {
            var section = $"Item: {ItemDef.name}";
            vialRegen = LITMain.config.Bind<float>(section, "Extra Regen Per Vial", 0.8f, "Extra Regeneration added per vial.").Value;
        }*/

        /*public override void DescriptionToken()
        {
            LITUtil.AddTokenToLanguage(ItemDef.descriptionToken,
                $"Regenerate an extra <style=cIsHealing>+{vialRegen}</style> <style=cStack>(+{vialRegen} per stack)</style> <style=cIsHealing>hp</style> per second.",
                LangEnum.en);
        }*/
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
