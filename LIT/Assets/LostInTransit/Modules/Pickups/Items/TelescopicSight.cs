using Moonstorm;
using RoR2;

namespace LostInTransit.Items
{
    public class TelescopicSight : ItemBase
    {
        private const string token = "LIT_ITEM_TELESCOPICSIGHT_DESC";
        public override ItemDef ItemDef { get; set; } = LITAssets.Instance.MainAssetBundle.LoadAsset<ItemDef>("TelescopicSight");

        public static string section;

        [ConfigurableField(ConfigName = "Base Proc Chance", ConfigDesc = "Base proc chance for Telescopic Sight.")]
        [TokenModifier(token, StatTypes.Default, 0)]
        public static float newBaseChance = 1f;

        [ConfigurableField(ConfigName = "Proc Chance per Stack", ConfigDesc = "Extra proc chance per stack of sights.")]
        [TokenModifier(token, StatTypes.Default, 1)]
        public static float newStackChance = 0.5f;

        [ConfigurableField(ConfigName = "Cooldown", ConfigDesc = "Cooldown between Telescopic Sight activations.")]
        [TokenModifier(token, StatTypes.Default, 3)]
        public static float teleCooldown = 20f;

        [ConfigurableField(ConfigName = "Cooldown Reduction per Stack", ConfigDesc = "Seconds removed from cooldown per stack.")]
        [TokenModifier(token, StatTypes.Default, 4)]
        public static float teleCooldownStack = 2f;

        [ConfigurableField(ConfigName = "Health Percentage Dealt to Exceptions", ConfigDesc = "Percentage of max health that's dealt to set exceptions when activated on them.")]
        [TokenModifier(token, StatTypes.Default, 2)]
        public static float exceptionHealthPercentage = 20f;

        [ConfigurableField(ConfigName = "Instakill Elites", ConfigDesc = "Whether Telescopic Sight should instakill elites.")]
        public static bool instakillElites = false;

        [ConfigurableField(ConfigName = "Instakill Bosses", ConfigDesc = "Whether Telescopic Sight should instakill boss monsters.")]
        public static bool instakillBosses = false;

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<TelescopicSightBehavior>(stack);
        }

        public class TelescopicSightBehavior : CharacterBody.ItemBehavior, IOnIncomingDamageOtherServerReciever
        {
            public void OnIncomingDamageOther(HealthComponent victimHealthComponent, DamageInfo damageInfo)
            {
                if (damageInfo.dotIndex == DotController.DotIndex.None)
                {
                    if (Util.CheckRoll(CalcChance() * damageInfo.procCoefficient) && !body.HasBuff(Buffs.TeleSightCD.buff))
                    {
                        body.AddCooldownBuff(Buffs.TeleSightCD.buff, CalcCooldown());
                        var flag = ChooseWetherToInstakill(victimHealthComponent.body);
                        if (flag)
                        {
                            damageInfo.damage = victimHealthComponent.body.maxHealth * 4;
                        }
                        else
                        {
                            damageInfo.damage = victimHealthComponent.body.maxHealth * (exceptionHealthPercentage / 100);
                        }
                        Util.PlaySound("TeleSightProc", body.gameObject);
                    }
                }
            }
            private float CalcChance()
            {
                float stackChance = newStackChance * (stack - 1);
                return newBaseChance + stackChance;
            }
            private float CalcCooldown()
            {
                //Yknow, we should NEVER reach a cooldown of 0, so this caps the cooldown at around 10 seconds.
                return teleCooldown - ((1 - 1 / (1 + 0.25f * (stack - 1))) * 10);
                //Agreeable.
            }
            /*
             * This code dictates wether the body can be instakilled or not, based off the config and the CharacterBody.
             * By default, only normal enemies & elites should be instakillable, Bosses are treated as exceptions.
             */
            //Hey, English lesson: "whether". There's no need for the 'h', but it's there for... some reason.
            //I fucking hate english.
            private bool ChooseWetherToInstakill(CharacterBody body)
            {
                if (body.isChampion)
                {
                    return instakillBosses;
                }
                if (body.isElite)
                {
                    return instakillElites;
                }
                return true;
            }
        }
    }
}
