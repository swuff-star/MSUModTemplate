using LostInTransit.Modules;
using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostInTransit.Buffs
{
    public class LeechingRegen : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("LeechingRegen");

        public static BuffDef leechingRegenDef;

        public override void Initialize()
        {
            leechingRegenDef = BuffDef;
            var croco = Resources.Load<BuffDef>("buffdefs/CrocoRegen");
            BuffDef.iconSprite = croco.iconSprite;
            BuffDef.startSfx = croco.startSfx;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<LeechingRegenBehavior>(stack);
        }

        public class LeechingRegenBehavior : CharacterBody.ItemBehavior, IStatItemBehavior
        {
            public float duration = 5;
            public float regen = 0;

            public GameObject VFX = Assets.LITAssets.LoadAsset<GameObject>("VFXLeechingRegen");
            public void Start()
            {
                if(body.isChampion)
                {
                    duration = 10;
                }
                VFX.GetComponent<DestroyOnTimer>().duration = duration;
                EffectData effectData = new EffectData
                {
                    scale = body.bestFitRadius,
                    origin = body.aimOrigin,
                    rootObject = body.gameObject
                };
                Debug.Log("SpawningEffect");
                EffectManager.SpawnEffect(VFX, effectData, true);
                CalculateRegen();
                body.statsDirty = true;
            }
            private void CalculateRegen()
            {
                regen = (body.maxHealth / 10) / duration;
            }
            public void RecalcStatsEnd()
            {
                
            }

            public void RecalcStatsStart()
            {
                body.regen += regen;
            }
        }
    }
}

