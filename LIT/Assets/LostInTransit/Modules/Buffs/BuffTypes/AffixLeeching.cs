using RoR2;
using System.Collections.Generic;
using Moonstorm;
using UnityEngine;
using System.Linq;

namespace LostInTransit.Buffs
{
    public class AffixLeeching : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("AffixLeeching");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<AffixLeechingBehavior>(stack);
        }

        public class AffixLeechingBehavior : CharacterBody.ItemBehavior, IOnDamageDealtServerReceiver
        {
            public float timeBetweenHeals = 20;

            public GameObject HealingEffect = Assets.LITAssets.LoadAsset<GameObject>("EffectLeechingBurst");

            public GameObject AbilityEffect = Assets.LITAssets.LoadAsset<GameObject>("EffectLeechingAbility");

            public GameObject TracerEffect = Assets.LITAssets.LoadAsset<GameObject>("TracerLeeching");

            private List<HealthComponent> healthComponents;

            private SphereSearch healSearch;

            private float stopwatch;

            private bool doingAbility;

            private float AbilityStopwatch;

            public void Awake()
            {
                healSearch = new SphereSearch();
                healthComponents = new List<HealthComponent>();
                var component = AbilityEffect.AddComponent<DestroyOnTimer>();
                component.duration = 20;
            }
            public void Update()
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
            }

            internal void Ability()
            {
                doingAbility = true;
                EffectData effectData = new EffectData
                {
                    scale = body.bestFitRadius,
                    origin = body.aimOrigin,
                    rootObject = body.gameObject
                };
                EffectManager.SpawnEffect(AbilityEffect, effectData, true);
            }
            private void HealNearby()
            {
                var hasBursted = false;
                SearchAllies();
                foreach(HealthComponent healthComponent in healthComponents)
                {
                    var charBody = healthComponent.body;
                    if(charBody != body && !charBody.HasBuff(LeechingRegen.buff) && !charBody.GetComponent<AffixLeechingBehavior>())
                    {
                        charBody.AddTimedBuff(LeechingRegen.buff, 5);
                        SpawnTracer(charBody.corePosition, body.corePosition);
                        if(!hasBursted)
                        {
                            EffectData effectData = new EffectData
                            {
                                scale = body.radius,
                                origin = body.aimOrigin
                            };
                            EffectManager.SpawnEffect(HealingEffect, effectData, true);
                            hasBursted = true;
                        }
                    }
                }
            }
            private void SearchAllies()
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
            }
            public void OnDamageDealtServer(DamageReport damageReport)
            {
                if(!doingAbility)
                {
                    damageReport.attackerBody?.healthComponent?.Heal(damageReport.damageDealt * damageReport.damageInfo.procCoefficient, default);
                }
                else
                {
                    SearchAllies();
                    float healing = (damageReport.damageDealt * 1.5f) / healthComponents.Count;
                    foreach(HealthComponent component in healthComponents)
                    {
                        SpawnTracer(component.body.corePosition, damageReport.attackerBody.corePosition);
                        component.Heal(healing, default);
                    }
                }
            }
            private void SpawnTracer(Vector3 origin, Vector3 start)
            {
                EffectData effectData = new EffectData
                {
                    origin = origin,
                    start = start
                };
                EffectManager.SpawnEffect(TracerEffect, effectData, true);
            }
        }
    }
}
