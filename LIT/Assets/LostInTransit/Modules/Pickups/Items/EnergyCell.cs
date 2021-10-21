using RoR2;
using Moonstorm;
using System;
using UnityEngine;

namespace LostInTransit.Items
{
    //[DisabledContent]
    public class EnergyCell : LITItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("EnergyCell");

        public static float bonusAttackSpeed;
        public override void Initialize()
        {
            Config();
            DescriptionToken();
        }

        public override void Config()
        {
            var section = $"Item: {ItemDef.name}";
            bonusAttackSpeed = LITMain.config.Bind<float>(section, "Maximum Attack Speed per Energy Cell", 0.4f, "Maximum amount of attack speed per item held.").Value;
        }

        public override void DescriptionToken()
        {
            LITUtil.AddTokenToLanguage(ItemDef.descriptionToken,
                $"Gain <style=cIsDamage>attack speed</style> proportionate to <style=cIsHealth>percentage of missing health</style>, up to a maximum of <style=cIsDamage>+{bonusAttackSpeed * 100}%</style> <style=cStack>(+{bonusAttackSpeed * 100}% per stack)</style> <style=cIsDamage>attack speed</style> with <style=cIsHealth>10% or less health remaining</style>.",
                LangEnum.en);
        }
        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<EnergyCellBehavior>(stack);
        }

        public class EnergyCellBehavior : CharacterBody.ItemBehavior, IStatItemBehavior, IOnIncomingDamageServerReceiver
        {
            //★. ..will look up and shout "stop doing everything in the FixedUpdate method!"... and I'll look down and whisper "no".
            //★ Jokes aside, this makes sense to do inside FixedUpdate, right? I figure doing it in RecalculateStats wouldn't update properly, since... well, it's only when RecalculateStats is called.
            //★ P.S. What do you call "FixedUpdate()"? Like, the name for it? It's a 'method', right? I am adding things inside of the method?
            public float missingHealthPercent;
            public float healthFraction;

            public void OnIncomingDamageServer(DamageInfo damageInfo)
            {
                body.RecalculateStats();
            }

            public void RecalculateStatsEnd()
            {
                body.attackSpeed += body.attackSpeed * (1 - healthFraction) * (float)(Math.Pow(bonusAttackSpeed, 1/stack));
            }

            public void RecalculateStatsStart()
            {
            }

            private void FixedUpdate()
            {
                missingHealthPercent = (1 - body.healthComponent.combinedHealthFraction);
                healthFraction = body.healthComponent.combinedHealthFraction - 0.1f;
                if (body.healthComponent.combinedHealthFraction > 0.9f)
                {
                    healthFraction = 1;
                }
                if (body.healthComponent.combinedHealthFraction < 0f)
                {
                    healthFraction = 0;
                }
                //★ Is there a better way to do this? From what I understand, Math.Floor() and Math.Ceil() are used to round numbers, rather than prevent them from exiting a specific range.
            }
        }
    }
}
