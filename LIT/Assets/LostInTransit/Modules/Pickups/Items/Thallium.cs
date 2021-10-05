using LostInTransit.Modules;
using RoR2;
using LostInTransit.Buffs;
using Moonstorm;

namespace LostInTransit.Items
{
    public class Thallium : LITItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("Thallium");

        public static string section;
        public static float procChance;
        public static float dmgCoefficient;
        public static float dmgStack;
        public static float slowMultiplier;
        public static int duration;
        public override void Initialize()
        {
            Config();
            DescriptionToken();
        }

        public override void Config()
        {
            section = "Item: " + ItemDef.name;
            procChance = LITMain.config.Bind<float>(section, "Proc Chance", 10f, "Chance to afflict Thallium Poisoning.").Value;
            dmgCoefficient = LITMain.config.Bind<float>(section, "Base Damage", 1.25f, "Damage coefficient of Thallium, multiplied by duration for total damage.").Value;
            dmgStack = LITMain.config.Bind<float>(section, "Stacking Damage", 0.625f, "Extra damage dealt by extra stacks.").Value;
            slowMultiplier = LITMain.config.Bind<float>(section, "Slow Multiplier", 0.25f, "Multiplier applied to the inflicted body's movement speed.").Value;
            duration = LITMain.config.Bind<int>(section, "DoT Duration", 4, "Duration of the Thallium Poisoning debuff.").Value;
        }

        public override void DescriptionToken()
        {
            LITUtil.AddTokenToLanguage(ItemDef.descriptionToken,
                $"<style=cIsDamage>10%</style> chance to inflict thallium poisoning for <style=cIsDamage>500%</style> <style=cStack>(+250% per stack)</style> of the victim's base damage and slow by <style=cIsUtility>75% movement speed</style>.",
                LangEnum.en);
        }
        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<ThalliumBehavior>(stack);
        }

        public class ThalliumBehavior : CharacterBody.ItemBehavior, IOnDamageDealtServerReceiver
        {
            public void OnDamageDealtServer(DamageReport damageReport)
            {
                var victim = damageReport.victim;
                var dotController = DotController.FindDotController(victim.gameObject);
                bool flag = false;
                if (dotController)
                    flag = dotController.HasDotActive(ThalliumPoison.index);

                if (Util.CheckRoll(procChance * damageReport.damageInfo.procCoefficient) && !flag)
                {
                    var dotInfo = new InflictDotInfo()
                    {
                        attackerObject = victim.gameObject,
                        victimObject = victim.gameObject,
                        dotIndex = ThalliumPoison.index,
                        duration = duration,
                        damageMultiplier = dmgCoefficient + dmgStack * (stack -1)
                    };
                    DotController.InflictDot(ref dotInfo);
                }
            }
        }
    }
}
