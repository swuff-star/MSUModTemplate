using RoR2;
using Moonstorm;
using LostInTransit.Buffs;

namespace LostInTransit.Items
{
    public class TelescopicSight : ItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("TelescopicSight");

        public static string section;
        public static float ExtraCrit;
        public static float NewBaseChance;
        public static float NewStackChance;
        public static float BossHealthPercentage;
        public static bool BossInstakill;
        public static float TeleCooldown;
        public static float TeleCooldownStack;

        public override void Initialize()
        {
            section = "Item: " + ItemDef.name;
            //ExtraCrit = LITMain.config.Bind<float>(section, "Extra Crit Chance", 10f, "Amount of flat critical chance the item gives.").Value;
            NewBaseChance = LITMain.config.Bind<float>(section, "Base Proc Chance", 1f, "Base Proc chance for Telescopic Sight.").Value;
            NewStackChance = LITMain.config.Bind<float>(section, "Stack Proc Chance", 0.5f, "Added Proc Chance per Stack.").Value;
            TeleCooldown = LITMain.config.Bind<float>(section, "Cooldown", 20f, "Cooldown until Telescopic Sight can proc a second time.").Value;
            TeleCooldownStack = LITMain.config.Bind<float>(section, "Cooldown Reduction per Stack", 2f, "Seconds removed from the cooldown per stack.").Value;
            BossHealthPercentage = LITMain.config.Bind<float>(section, "Boss Health Percentage", 20f, "Percent of max health that is dealt to bosses.").Value;
            BossInstakill = LITMain.config.Bind<bool>(section, "Instakill Bosses", false, "Whether the Telescopic Sight should instakill bosses.").Value;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<TelescopicSightBehavior>(stack);
        }

        public class TelescopicSightBehavior : CharacterBody.ItemBehavior, IStatItemBehavior, IOnDamageDealtServerReceiver
        {

            public void RecalculateStatsStart() { }
            public void RecalculateStatsEnd()
            {
            }

            //Euthanasia Behavior
            public void OnDamageDealtServer(DamageReport report)
            {
                if (!R2API.DamageAPI.HasModdedDamageType(report.damageInfo, DamageTypes.Hypercrit.hypercritDamageType) && report.dotType == DotController.DotIndex.None)
                {
                    if (Util.CheckRoll(CalcChance() * report.damageInfo.procCoefficient) && body.GetBuffCount(TeleSightCD.buff) < 1)
                    {
                        body.AddTimedBuff(TeleSightCD.buff, TeleCooldown - (stack - 1) * TeleCooldownStack);
                        //Bosses recieve 3 times the damage.
                        if (report.victimIsBoss && !BossInstakill)
                        {
                            DamageInfo newDamageInfo = report.damageInfo;
                            R2API.DamageAPI.AddModdedDamageType(newDamageInfo, DamageTypes.Hypercrit.hypercritDamageType);
                            newDamageInfo.damage = report.victimBody.maxHealth / BossHealthPercentage;
                            report.victimBody.healthComponent.TakeDamage(newDamageInfo);
                        }
                        else
                        //other enemies get insta killed.
                        {
                            DamageInfo instakill = new DamageInfo
                            {
                                attacker = report.damageInfo.attacker,
                                crit = report.attackerBody.RollCrit(),
                                damage = report.victimBody.maxHealth * 3,
                                damageColorIndex = DamageColorIndex.DeathMark,
                                damageType = DamageType.Generic,
                                dotIndex = DotController.DotIndex.None,
                                force = report.damageInfo.force,
                                inflictor = report.damageInfo.inflictor,
                                position = report.damageInfo.position,
                                procChainMask = report.damageInfo.procChainMask,
                                procCoefficient = report.damageInfo.procCoefficient,
                            };
                            report.victimBody.healthComponent.TakeDamage(instakill);
                        }
                    }
                }
            }

            private float CalcChance()
            {
                float chance;
                float baseChance = NewBaseChance;
                float stackChance = NewStackChance * (stack - 1);
                //baseChance /= 100;
                //stackChance /= 100;

                //This rougly equates to 9.09% chance on 1 telesight. Hyperbolic so it never reaches 100%.
                //chance = (1 - 1 / (1 + (baseChance + (stackChance * (stack - 1)))) * 100);
                //I should be able to figure out how this math works but I simply cannot. Just making it linear like RoR1 for now, reaching 3% is pretty much impossible in a non-cheated run anyhow.
                chance = baseChance + stackChance;

                return chance;
            }

            //Original behavior
            /*public void OnDamageDealtServer(DamageReport report)
            {
                //Doing this damageapi stuff is extremely retarded
                if (!R2API.DamageAPI.HasModdedDamageType(report.damageInfo, DamageTypes.Hypercrit.hypercritDamageType) && report.dotType == DotController.DotIndex.None && report.damageInfo.crit)
                {
                    if (Util.CheckRoll(BaseChance + (StackChance * (stack - 1))))
                    {
                        DamageInfo newDamageInfo = report.damageInfo;
                        R2API.DamageAPI.AddModdedDamageType(newDamageInfo, DamageTypes.Hypercrit.hypercritDamageType);
                        newDamageInfo.damage = report.damageInfo.damage * 4;
                        report.victimBody.healthComponent.TakeDamage(newDamageInfo);
                    }
                }
            }*/
        }
    }
}
