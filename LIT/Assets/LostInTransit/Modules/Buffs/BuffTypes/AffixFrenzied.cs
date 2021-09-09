using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LostInTransit.Modules;
using UnityEngine.Networking;
using LostInTransit.Utils;
using LostInTransit.Components;

namespace LostInTransit.Buffs
{
    public class AffixFrenzied : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("AffixFrenzied");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<AffixFrenziedBehavior>(stack);
            body.RecalculateStats();
        }

        public class AffixFrenziedBehavior : CharacterBody.ItemBehavior, IStatItemBehavior
        {
            public float timeBetweenHeals = 20;

            //public GameObject HealingEffect = Assets.LITAssets.LoadAsset<GameObject>("VFXLeeching");

            //public GameObject AbilityEffect = Assets.LITAssets.LoadAsset<GameObject>("VFXLeechingAbilityActive");

            //public GameObject TracerEffect = Assets.LITAssets.LoadAsset<GameObject>("TracerLeeching");

            private List<HealthComponent> healthComponents;

            private SphereSearch healSearch;

            private float stopwatch;

            private bool doingAbility;

            private float AbilityStopwatch;

            public void Awake()
            {
                //healSearch = new SphereSearch();
                //healthComponents = new List<HealthComponent>();
                //var component = AbilityEffect.AddComponent<DestroyOnTimer>();
                //component.duration = 20;
                body.RecalculateStats();
            }

            public void RecalcStatsEnd()
            {
                body.attackSpeed += body.attackSpeed * 5.5f;
                body.moveSpeed += body.moveSpeed * 5.5f;
            }

            public void RecalcStatsStart()
            {

            }

            /*public void Update()
{
   stopwatch += Time.deltaTime;
   if(stopwatch >= timeBetweenHeals)
   {
       stopwatch = 0;
       HealNearby();
   }
   if(doingAbility)
   {
       AbilityStopwatch += Time.deltaTime;
       if(AbilityStopwatch >= 20)
       {
           doingAbility = false;
           AbilityStopwatch = 0;
       }
   }
}*/

            /*internal void Ability()
            {
                doingAbility = true;
                EffectData effectData = new EffectData
                {
                    scale = body.bestFitRadius,
                    origin = body.aimOrigin,
                    rootObject = body.gameObject
                };
                EffectManager.SpawnEffect(AbilityEffect, effectData, true);
            }*/
            /*private void HealNearby()
            {
                var hasBursted = false;
                SearchAllies();
                float newTime = 20;
                float timeMult = 1.0f;
                foreach(HealthComponent healthComponent in healthComponents)
                {
                    if(healthComponent.body != body && !healthComponent.body.HasBuff(RoR2Content.Buffs.CrocoRegen))
                    {
                        if(healthComponent.body.isChampion)
                        {
                            healthComponent.body.AddTimedBuff(RoR2Content.Buffs.CrocoRegen, 10);
                            timeMult += 0.2f;
                        }
                        else
                        {
                            healthComponent.body.AddTimedBuff(RoR2Content.Buffs.CrocoRegen, 5);
                            timeMult += 0.1f;
                        }
                        SpawnTracer(healthComponent.body.corePosition, body.corePosition);
                        if(!hasBursted)
                        {
                            EffectData effectData = new EffectData
                            {
                                scale = body.radius,
                                origin = body.aimOrigin
                            };
                            //EffectManager.SpawnEffect(HealingEffect, effectData, true);
                            hasBursted = true;
                        }
                    }
                    timeBetweenHeals = newTime * timeMult;
                }
            }*/
            /*private void SearchAllies()
            {
                List<HurtBox> hurtBoxes = new List<HurtBox>();
                TeamMask mask = default(TeamMask);
                mask.AddTeam(body.teamComponent.teamIndex);
                healSearch.mask = LayerIndex.entityPrecise.mask;
                healSearch.radius = 20;
                healSearch.origin = body.corePosition;
                healSearch.RefreshCandidates();
                healSearch.FilterCandidatesByHurtBoxTeam(mask);
                healSearch.GetHurtBoxes(hurtBoxes);
                healthComponents.Clear();
                foreach(HurtBox h in hurtBoxes)
                {
                    if(!healthComponents.Contains(h.healthComponent) && h.healthComponent.health < h.healthComponent.fullHealth)
                    {
                        healthComponents.Add(h.healthComponent);
                    }
                }
            }*/

            /*private void SpawnTracer(Vector3 origin, Vector3 start)
            {
                EffectData effectData = new EffectData
                {
                    origin = origin,
                    start = start
                };
                //EffectManager.SpawnEffect(TracerEffect, effectData, true);
            }*/
        }
    }
}
