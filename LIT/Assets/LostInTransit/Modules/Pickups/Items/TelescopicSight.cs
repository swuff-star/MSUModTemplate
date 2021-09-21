using RoR2;
using Moonstorm;

namespace LostInTransit.Items
{
    public class TelescopicSight : ItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("TelescopicSight");

        public static string section;
        public static float ExtraCrit;
        public static float BaseChance;
        public static float StackChance;
        public static float DamageMultiplier;

        public override void Initialize()
        {
            section = "Item: " + ItemDef.name;
            ExtraCrit = LITMain.config.Bind<float>(section, "Extra Crit Chance", 10f, "Amount of flat critical chance the item gives.").Value;
            BaseChance = LITMain.config.Bind<float>(section, "Base Proc Chance", 10f, "Base Proc chance for Telescopic Sight.").Value;
            StackChance = LITMain.config.Bind<float>(section, "Stack Proc Chance", 10f, "Added Proc Chance per Stack.").Value;
            DamageMultiplier = LITMain.config.Bind<float>(section, "Boss Damage Multiplier", 3f, "Extra damage dealt to bosses instead of insta-killing them.").Value; /*LITMain.config.Bind<float>(section, "Damage Multiplier", 4f, "Amount of extra damage added to procs, x 100.").Value;*/
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<TelescopicSightBehavior>(stack);
        }

        //Todo: make this not shit, i would really like to implement it like isaac's euthanasia
        public class TelescopicSightBehavior : CharacterBody.ItemBehavior, IStatItemBehavior, IOnDamageDealtServerReceiver
        {

            public void RecalculateStatsStart() { }
            public void RecalculateStatsEnd()
            {
                body.crit += ExtraCrit;
            }

            //Euthanasia Behavior
            public void OnDamageDealtServer(DamageReport report)
            {
                if (!R2API.DamageAPI.HasModdedDamageType(report.damageInfo, DamageTypes.Hypercrit.hypercritDamageType) && report.dotType == DotController.DotIndex.None && report.damageInfo.crit)
                {
                    if (Util.CheckRoll(CalcChance() * report.damageInfo.procCoefficient))
                    {
                        //Bosses recieve 3 times the damage.
                        if (report.victimIsBoss)
                        {
                            DamageInfo newDamageInfo = report.damageInfo;
                            R2API.DamageAPI.AddModdedDamageType(newDamageInfo, DamageTypes.Hypercrit.hypercritDamageType);
                            newDamageInfo.damage = report.damageInfo.damage * DamageMultiplier;
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
                float baseChance = BaseChance;
                float stackChance = StackChance * (stack - 1);
                baseChance /= 100;
                stackChance /= 100;

                //This rougly equates to 9.09% chance on 1 telesight. Hyperbolic so it never reaches 100%.
                chance = (1 - 1 / (1 + (baseChance + stackChance) * stack)) * 100;

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
