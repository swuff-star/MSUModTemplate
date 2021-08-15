using LostInTransit.Modules;
using RoR2;
using LostInTransit.Buffs;

namespace LostInTransit.Items
{
    public class Thallium : ItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("Thallium");

        public static float procChance;
        public static float dmgCoefficient;
        public static float dmgStack;
        public static float slowMultiplier;
        public static int duration;
        public override void Initialize()
        {
            procChance = LITMain.config.Bind<float>("Item: " + ItemDef.name, "Proc Chance", 10f, "Chance to afflict Thallium Poisoning.").Value;
            dmgCoefficient = LITMain.config.Bind<float>("Item: " + ItemDef.name, "Base Damage", 1.25f, "Damage coefficient of Thallium, multiplied by duration for total damage.").Value;
            dmgStack = LITMain.config.Bind<float>("Item: " + ItemDef.name, "Stacking Damage", 0.625f, "Extra damage dealth by extra stacks.").Value;
            slowMultiplier = LITMain.config.Bind<float>("Item: " + ItemDef.name, "Slow Amount", 0.75f, "Power of Thallium Poisoning's slow.").Value;
            duration = LITMain.config.Bind<int>("Item: " + ItemDef.name, "Debuff Duration", 4, "Duration of the Thallium Poisoning debuff.").Value;
        }
        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<MysteriousVialBehavior>(stack);
        }

        public class MysteriousVialBehavior : CharacterBody.ItemBehavior, IOnDamageDealtServerReceiver
        {
            public void OnDamageDealtServer(DamageReport damageReport)
            {
                var body = damageReport.victim;
                var dotController = DotController.FindDotController(body.gameObject);
                bool hasDot = false;
                if (dotController)
                    hasDot = dotController.HasDotActive(ThalliumPoison.index);

                if (Util.CheckRoll(procChance) && !hasDot)
                {
                    var dotInfo = new InflictDotInfo()
                    {
                        attackerObject = body.gameObject,
                        victimObject = body.gameObject,
                        dotIndex = ThalliumPoison.index,
                        duration = duration,
                        damageMultiplier = damageReport.victimBody.damage * (dmgCoefficient + dmgStack * (stack - 1))
                    };
                    DotController.InflictDot(ref dotInfo);
                }
            }
        }
    }
}
