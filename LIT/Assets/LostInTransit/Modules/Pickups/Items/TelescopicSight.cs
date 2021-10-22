using Moonstorm;
using RoR2;
using UnityEngine;

namespace LostInTransit.Items
{
    public class TelescopicSight : ItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("TelescopicSight");

        public static string section;

        [ConfigurableField(ConfigName = "Base Proc Chance", ConfigDesc = "Base proc chance for Telescopic Sight.")]
        public static float newBaseChance = 1f;

        [ConfigurableField(ConfigName = "Proc Chance per Stack", ConfigDesc = "Extra proc chance per stack of sights.")]
        public static float newStackChance = 0.5f;

        [ConfigurableField(ConfigName = "Cooldown", ConfigDesc = "Cooldown between Telescopic Sight activations.")]
        public static float TeleCooldown = 20f;

        [ConfigurableField(ConfigName = "Cooldown Reduction per Stack", ConfigDesc = "Seconds removed from cooldown per stack.")]
        public static float TeleCooldownStack = 2f;

        [ConfigurableField(ConfigName = "Health Percentage Dealt to Exceptions", ConfigDesc = "Percentage of max health that's dealt to set exceptions when activated on them.")]
        public static float ExceptionHealthPercentage = 20f;

        [ConfigurableField(ConfigName = "Instakill Bosses", ConfigDesc = "Whether Telescopic Sight should instakill boss monsters.")]
        public static bool InstakillElites = false;

        [ConfigurableField(ConfigName = "Instakill Elites", ConfigDesc = "Whether Telescopic Sight should instakill elites.")]
        public static bool InstakillBosses = true;

        /*
        public override void Config()
        {
            section = "Item: " + ItemDef.name;
            newBaseChance = LITMain.config.Bind<float>(section, "Base Proc Chance", 1f, "Base Proc Chance for Telescopic Sight.").Value;
            newStackChance = LITMain.config.Bind<float>(section, "Stack Proc Chance", 0.5f, "Stack proc chance for each telescopic sight.").Value;
            TeleCooldown = LITMain.config.Bind<float>(section, "Cooldown", 20f, "Cooldown until Telescopic Sight can proc again.").Value;
            TeleCooldownStack = LITMain.config.Bind<float>(section, "Cooldown Reduction Per Stack", 2f, "Seconds removed from the cooldown per stack.").Value;
            ExceptionHealthPercentage = LITMain.config.Bind<float>(section, "Exceptions Health Percentage", 20f, "Percentage of max health that's dealt to set exceptions.").Value;
            InstakillBosses = LITMain.config.Bind<bool>(section, "Instakill Bosses", false, "Whether Telescopic Sight should instakill boss monsters.").Value;
            InstakillElites = LITMain.config.Bind<bool>(section, "Instakill Elites", true, "Whether Telescopic Sight should instakill elite monsters.").Value;
        }*/
        /*
        public override void DescriptionToken()
        {
            LITUtil.AddTokenToLanguage(ItemDef.descriptionToken,
                $"<style=cIsDamage>{newBaseChance}%</style> <style=cStack>(+{newStackChance}% per stack)</style> chance to <style=cIsDamage>instakill monsters</style>. Boss monsters instead take <style=cIsDamage>{ExceptionHealthPercentage}% of their maximum health</style> in damage. Recharges every <style=cIsUtility>{TeleCooldown}</style> <style=cStack>(-{TeleCooldownStack} per stack)</style> seconds.",
                LangEnum.en);
        }*/

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<TelescopicSightBehavior>(stack);
        }

        public class TelescopicSightBehavior : CharacterBody.ItemBehavior, IOnIncomingDamageOtherServerReciever
        {
            public void OnIncomingDamageOther(HealthComponent victimHealthComponent, DamageInfo damageInfo)
            {
                if(damageInfo.dotIndex == DotController.DotIndex.None)
                {
                    if(Util.CheckRoll(CalcChance() * damageInfo.procCoefficient) && !body.HasBuff(Buffs.TeleSightCD.buff))
                    {
                        body.AddCooldownBuff(Buffs.TeleSightCD.buff, CalcCooldown());
                        var flag = ChooseWetherToInstakill(victimHealthComponent.body);
                        if(flag)
                        {
                            damageInfo.damage = victimHealthComponent.body.maxHealth * 4;
                        }
                        else
                        {
                            damageInfo.damage = victimHealthComponent.body.maxHealth * (ExceptionHealthPercentage / 100);
                        }
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
                return TeleCooldown - ((1 - 1 / (1 + 0.25f * (stack - 1))) * 10);
                //Agreeable.
            }
            /*
             * This code dictates wether the body can be instakilled or not, based off the config and the CharacterBody.
             * By default, only normal enemies & elites should be instakillable, Bosses are treated as exceptions.
             */
            //Hey, English lesson: "whether". There's no need for the 'h', but it's there for... some reason.
            private bool ChooseWetherToInstakill(CharacterBody body)
            {

                if(body.isChampion)
                {
                    if (InstakillBosses)
                        return true;
                    else
                        return false;
                }
                if(body.isElite)
                {
                    if(InstakillElites)
                        return true;
                    else
                        return false;
                }
                return true;
            }
        }
    }
}
