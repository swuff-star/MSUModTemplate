using LostInTransit.Buffs;
using Moonstorm;
using RoR2;

namespace LostInTransit.Items
{
    public class Thallium : ItemBase
    {
        public const string token = "LIT_ITEM_THALLIUM_DESC";
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("Thallium");

        public static string section;

        [ConfigurableField(ConfigName = "Proc Chance", ConfigDesc = "Chance to afflict Thallium Poisoning.")]
        [TokenModifier(token, StatTypes.Default, 0)]
        public static float procChance = 10f;

        [ConfigurableField(ConfigName = "Base Damage", ConfigDesc = "Damage coefficient of Thallium, multiplied by duration for total damage.")]
        public static float dmgCoefficient = 1.25f;

        [ConfigurableField(ConfigName = "Stacking Damage", ConfigDesc = "Extra damage dealt by extra stacks of Thallium.")]
        public static float newDmgStack = 0f;

        [ConfigurableField(ConfigName = "Slow Multiplier", ConfigDesc = "How much inflicted bodies are slowed by.")]
        [TokenModifier(token, StatTypes.Percentage, 3)]
        public static float newSlowMultiplier = 0.75f;

        [ConfigurableField(ConfigName = "Poisoning Duration", ConfigDesc = "Duration of the Thallium Poisoning debuff.")]
        public static int duration = 4;

        [ConfigurableField(ConfigName = "Poisoning Duration per Stack", ConfigDesc = "Duration added to the Thallium Poisoning debuff per stack.")]
        public static int durationStack = 2;

        //this is dumb but it is what it is. caused by the final damage being a math equation of dmgCoef multiplied by duration, fuck u swoof
        #region dumb
        [TokenModifier(token, StatTypes.Percentage, 1)]
        public static float damageWithDuration = dmgCoefficient * duration;
        [TokenModifier(token, StatTypes.Percentage, 2)]
        public static float damageWithDurationStack = dmgCoefficient * durationStack;
        #endregion

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
                        duration = duration + (durationStack * (stack - 1)),
                        damageMultiplier = dmgCoefficient + newDmgStack * (stack - 1)
                    };
                    DotController.InflictDot(ref dotInfo);
                    Util.PlaySound("ThalliumProc", body.gameObject);
                }
            }
        }
    }
}
