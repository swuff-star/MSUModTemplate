using RoR2;
using Moonstorm;

namespace LostInTransit.Items
{
    [DisabledContent]
    public class EnergyCell : LITItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("EnergyCell");

        public static float vialRegen;
        public override void Initialize()
        {
            Config();
            DescriptionToken();
            //vialRegen = LITMain.config.Bind<float>("Item: " + ItemDef.name, "Extra Regen Per Vial", 0.8f, "Extra health regeneration adder per vial.").Value;
        }

        public override void Config()
        {
            var section = $"Item: {ItemDef.name}";
            //vialRegen = LITMain.config.Bind<float>(section, "Extra Regen Per Vial", 0.8f, "Extra Regeneration added per vial.").Value;
        }

        public override void DescriptionToken()
        {
            LITUtil.AddTokenToLanguage(ItemDef.descriptionToken,
                $"Regenerate an extra <style=cIsHealing>+{vialRegen}</style> <style=cStack>(+{vialRegen} per stack)</style> <style=cIsHealing>hp</style> per second.",
                LangEnum.en);
        }
        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<EnergyCellBehavior>(stack);
        }

        public class EnergyCellBehavior : CharacterBody.ItemBehavior
        {
            //★. ..will look up and shout "stop doing everything in the FixedUpdate method!"... and I'll look down and whisper "no".
            //★ Jokes aside, this makes sense to do inside FixedUpdate, right? I figure doing it in RecalculateStats wouldn't update properly, since... well, it's only when RecalculateStats is called.
            //★ P.S. What do you call "FixedUpdate()"? Like, the name for it? It's a 'method', right? I am adding things inside of the method?
            private void FixedUpdate()
            { }
        }
    }
}
