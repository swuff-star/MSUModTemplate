using Moonstorm;
using RoR2;
using UnityEngine;

namespace LostInTransit.Items
{
    public class TelescopicSight : LITItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("TelescopicSight");

        public static string section;
        public static float BaseChance;
        public static float StackChance;
        public static float TeleCooldown;
        public static float TeleCooldownStack;
        public static float ExceptionHealthPercentage;
        public static bool InstakillElites;
        public static bool InstakillBosses;
        public override void Initialize()
        {
            Config();
            DescriptionToken();
        }

        public override void Config()
        {
            section = "Item: " + ItemDef.name;
            BaseChance = LITMain.config.Bind<float>(section, "Base Proc Chance", 1f, "Base Proc Chance for Telescopic Sight.").Value;
            StackChance = LITMain.config.Bind<float>(section, "Stack Proc Chance", 0.5f, "Stack proc chance for each telescopic sight.").Value;
            TeleCooldown = LITMain.config.Bind<float>(section, "Cooldown", 20f, "Cooldown until Telescopic Sight can proc again.").Value;
            TeleCooldownStack = LITMain.config.Bind<float>(section, "Cooldown Reduction Per Stack", 2f, "Seconds removed from the cooldown per stack.").Value;
            ExceptionHealthPercentage = LITMain.config.Bind<float>(section, "Exceptions Health Percentage", 20f, "Percentage of max health that's dealt to set exceptions.").Value;
            InstakillBosses = LITMain.config.Bind<bool>(section, "Instakill Bosses", false, "Whether Telescopic Sight should instakill boss monsters.").Value;
            InstakillElites = LITMain.config.Bind<bool>(section, "Instakill Elites", true, "Whether Telescopic Sight should instakill elite monsters.").Value;
        }
        //Good luck i guess, lol.
        public override void DescriptionToken()
        {
            LITUtil.AddTokenToLanguage(ItemDef.descriptionToken,
                $"<style=cIsDamage>{BaseChance}%</style> <style=cStack>(+{StackChance}% per stack)</style> chance to <style=cIsDamage>instakill monsters</style>. Boss monsters instead take <style=cIsDamage>{ExceptionHealthPercentage}% of their maximum health</style> in damage. Recharges every <style=cIsUtility>{TeleCooldown}</style> <style=cStack>(-{TeleCooldownStack} per stack)</style> seconds.",
                LangEnum.en);
        }

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
                float stackChance = StackChance * (stack - 1);
                return BaseChance + stackChance;
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
