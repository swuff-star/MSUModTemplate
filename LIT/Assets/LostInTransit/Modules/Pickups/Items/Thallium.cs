using LostInTransit.Buffs;
using Moonstorm;
using RoR2;
using UnityEngine;

namespace LostInTransit.Items
{
    public class Thallium : ItemBase
    {
        public const string token = "LIT_ITEM_THALLIUM_DESC";
        public override ItemDef ItemDef { get; } = LITAssets.Instance.MainAssetBundle.LoadAsset<ItemDef>("Thallium");

        public static string section;

        [ConfigurableField(ConfigName = "Proc Chance", ConfigDesc = "Chance to afflict Thallium Poisoning.")]
        [TokenModifier(token, StatTypes.Default, 0)]
        public static float procChance = 10f;

        [ConfigurableField(ConfigName = "Total Damage", ConfigDesc = "Total damage of Thallium, as a percentage of the victim's damage. Halved after the first stack")]
        [TokenModifier(token, StatTypes.Default, 1)]
        [TokenModifier(token, StatTypes.DivideBy2, 2)]
        public static float dmgCoefficient = 500f;

        [ConfigurableField(ConfigName = "Slow Multiplier", ConfigDesc = "How much the victim is slowed by.")]
        [TokenModifier(token, StatTypes.Default, 3)]
        public static float slowMultiplier = 75f;

        [ConfigurableField(ConfigName = "Poisoning Duration", ConfigDesc = "Amount of time needed to deal the full damage. By default, increases with stacks. Minimum 1.")]
        public static int duration = 4;

        [ConfigurableField(ConfigName = "Poison is Fixed Duration", ConfigDesc = "If enabled, stacks increase the damage per tick instead of the total duration")]
        public static bool noTimeToDie = false;

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<ThalliumBehavior>(stack);
        }

        public class ThalliumBehavior : CharacterBody.ItemBehavior, IOnDamageDealtServerReceiver
        {
            public void OnDamageDealtServer(DamageReport damageReport)
            {
                var attacker = damageReport.attacker;
                var victim = damageReport.victim;
                var dotController = DotController.FindDotController(victim.gameObject);
                bool flag = false;
                if (dotController)
                    flag = dotController.HasDotActive(ThalliumPoison.index);

                if (Util.CheckRoll(procChance * damageReport.damageInfo.procCoefficient) && !flag)
                {
                    float newDuration = Mathf.Max(duration, 1f);
                    float newDamage = (dmgCoefficient / 100) * (1 + ((stack - 1) / 2));
                    if (!noTimeToDie)
                        newDuration += (stack - 1) * 2;
                    var dotInfo = new InflictDotInfo()
                    {
                        attackerObject = attacker.gameObject,
                        victimObject = victim.gameObject,
                        dotIndex = ThalliumPoison.index,
                        duration = newDuration,
                        //G - dividing by attacker damage = 1, then multiply by victim damage for corrected damage
                        damageMultiplier = (damageReport.victimBody.damage / damageReport.attackerBody.damage) * (newDamage / newDuration)
                    };
                    DotController.InflictDot(ref dotInfo);
                    Util.PlaySound("ThalliumProc", body.gameObject);
                }
            }
        }
    }
}
