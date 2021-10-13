using LostInTransit.Modules;
using RoR2;
using LostInTransit.Buffs;
using Moonstorm;

namespace LostInTransit.Items
{
    public class PrisonShackles : LITItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("PrisonShackles");

        public static string section;
        public static float slowMultiplier;
        public static int duration;
        public static int durationStack;
        public override void Initialize()
        {
            Config();
            DescriptionToken();
        }

        public override void Config()
        {
            section = "Item: " + ItemDef.name;
            slowMultiplier = LITMain.config.Bind<float>(section, "Slow Multiplier", 0.6f, "Multiplier applied to the inflicted body's movement speed.").Value;
            duration = LITMain.config.Bind<int>(section, "Duration", 2, "Base duration of the Shackled debuff.").Value;
            durationStack = LITMain.config.Bind<int>(section, "Stacking Duration", 2, "Duration of the Shackled debuff per stack.").Value;
        }

        public override void DescriptionToken()
        {
            LITUtil.AddTokenToLanguage(ItemDef.descriptionToken,
                $"<style=cIsUtility>Slow</style> enemies on hit for <style=cIsUtility>-{slowMultiplier * 100}% attack speed</style> for <style=cIsUtility>{duration}s</style> <style=cStack>(+{durationStack}s per stack)</style>.",
                LangEnum.en);
        }
        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<PrisonShacklesBehavior>(stack);
        }

        public class PrisonShacklesBehavior : CharacterBody.ItemBehavior, IOnDamageDealtServerReceiver
        {
            public void OnDamageDealtServer(DamageReport damageReport)
            {
                damageReport.victimBody.AddTimedBuff(Shackled.buff, duration + durationStack * (stack - 1));
            }
        }
    }
}
