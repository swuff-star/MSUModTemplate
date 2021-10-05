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
            TeleCooldownStack = LITMain.config.Bind<float>(section, "Cooldown Reduction Per Stack", 2f, "Seconds Removed from the cooldown per stack.").Value;
            ExceptionHealthPercentage = LITMain.config.Bind<float>(section, "Exceptions Health Percentage", 20f, "Health Percentage of remaining health that's dealt to Exceptions.").Value;
            InstakillBosses = LITMain.config.Bind<bool>(section, "Instakill Bosses", false, "Wether the Telescopic Sight should instakill bosses.\nIf false, The bosses are treated as Exceptions.").Value;
            InstakillElites = LITMain.config.Bind<bool>(section, "Instakill Elites", true, "Wether the Telescopic Sight should Instakill Elites.\nIf false, the Elites are treated as Exceptions.").Value;
        }
        //Good luck i guess, lol.
        public override void DescriptionToken()
        {
            LITUtil.AddTokenToLanguage(ItemDef.descriptionToken,
                $"Gain <style=cIsUtility>10% critical chance</style>. <style=cIsDamage>5%</style> <style=cStack>(+5% per stack)</style> chance for critical strikes to <style=cIsDamage>instantly kill</style> enemies. Bosses receive <style=cIsDamage>300%</style> TOTAL damage instead of being instantly killed.",
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
                            damageInfo.damage = victimHealthComponent.health * (ExceptionHealthPercentage / 100);
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
            }
            /*
             * This code dictates wether the body can be instakilled or not, based off the config and the CharacterBody.
             * By default, only normal enemies & elites should be instakillable, Bosses are treated as exceptions.
             */
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
