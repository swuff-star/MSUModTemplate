using Moonstorm;
using RoR2;
using UnityEngine;
using R2API;

namespace LostInTransit.Buffs
{
    public class LeechingRegen : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("LeechingRegen");

        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
            var croco = Resources.Load<BuffDef>("buffdefs/CrocoRegen");
            BuffDef.iconSprite = croco.iconSprite;
            BuffDef.startSfx = croco.startSfx;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<LeechingRegenBehavior>(stack);
        }

        public class LeechingRegenBehavior : CharacterBody.ItemBehavior, IBodyStatArgModifier
        {
            public float duration = 5;
            public float totalRegen = 0;
            public float regenStrength = 10;

            public GameObject VFX = Assets.LITAssets.LoadAsset<GameObject>("EffectLeechingRegen");
            public void Start()
            {
                VFX.GetComponent<DestroyOnTimer>().duration = duration;
                EffectData effectData = new EffectData
                {
                    scale = body.bestFitRadius / 10,
                    origin = body.aimOrigin,
                    rootObject = body.gameObject
                };
                EffectManager.SpawnEffect(VFX, effectData, true);
                totalRegen = body.maxHealth * (regenStrength / 100) / duration;
                LITLogger.LogI($"Increasing regen of {body} for {totalRegen}");
                body.RecalculateStats();
            }
            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.baseRegenAdd += totalRegen;
            }
        }
    }
}

