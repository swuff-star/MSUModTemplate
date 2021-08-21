using LostInTransit.Modules;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
            ExtraCrit = LITMain.config.Bind<float>(section, "Extra Crit Chance", 10f, "How much extra critical chance the item gives.").Value;
            BaseChance = LITMain.config.Bind<float>(section, "Base Proc Chance", 10f, "Proc chance for the Telescopic Sight to apply its damage multiplier.").Value;
            StackChance = LITMain.config.Bind<float>(section, "Stack Proc Chance", 10f, "Extra Proc Chance added with each Stack.").Value;
            DamageMultiplier = LITMain.config.Bind<float>(section, "Damage Multiplier", 4f, "Damage multiplier").Value;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<TelescopicSightBehavior>(stack);
        }

        public class TelescopicSightBehavior : CharacterBody.ItemBehavior, IStatItemBehavior, IOnDamageDealtServerReceiver
        {

            public void RecalcStatsStart() { }
            public void RecalcStatsEnd()
            {
                body.crit += ExtraCrit;
            }

            public void OnDamageDealtServer(DamageReport report)
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
            }
        }
    }
}
