using Moonstorm;
using RoR2;

namespace LostInTransit.Items
{
    public class TelescopicSight : ItemBase
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
            section = "Item: " + ItemDef.name;
            BaseChance = LITMain.config.Bind<float>(section, "Base Proc Chance", 1f, "Base Proc Chance for Telescopic Sight.").Value;
            StackChance = LITMain.config.Bind<float>(section, "Stack Proc Chance", 0.5f, "Stack proc chance for each telescopic sight.").Value;
            TeleCooldown = LITMain.config.Bind<float>(section, "Cooldown", 20f, "Cooldown until Telescopic Sight can proc again.").Value;
            TeleCooldownStack = LITMain.config.Bind<float>(section, "Cooldown Reduction Per Stack", 2f, "Seconds Removed from the cooldown per stack.").Value;
            ExceptionHealthPercentage = LITMain.config.Bind<float>(section, "Exceptions Health Percentage", 20f, "Health Percentage of remaining health that's dealt to Exceptions.").Value;
            InstakillElites = LITMain.config.Bind<bool>(section, "Instakill Elites", true, "Wether the Telescopic Sight should instakill elites.\nIf false, The elites are treated as Exceptions.").Value;
            InstakillBosses = LITMain.config.Bind<bool>(section, "Instakill Bosses", false, "Wether the Telescopic Sight should instakill bosses.\nIf false, The bosses are treated as Exceptions.").Value;

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
                        for(int i = 0; i <= TeleCooldown - (TeleCooldownStack * (stack - 1)); i++)
                        {
                            body.AddTimedBuff(Buffs.TeleSightCD.buff, i);
                        }
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
            /*
             * This code dictates wether the body can be instakilled or not, based off the config and the CharacterBody.
             * By default, only normal enemies should be instakillable, Elites and Bosses are treated as exceptions.
             */
            private bool ChooseWetherToInstakill(CharacterBody body)
            {
                bool shouldKillBoss = InstakillBosses;
                bool shouldKillElite = InstakillElites;

                if (body.isElite && shouldKillElite)
                    return true;
                else if (body.isBoss && shouldKillBoss)
                    return true;
                return false;
            }
        }
    }
}
