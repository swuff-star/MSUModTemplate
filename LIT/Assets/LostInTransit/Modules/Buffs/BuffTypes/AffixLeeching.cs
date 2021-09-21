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
            public static float timeBetweenHeals = 10;

            public static float abilityDuration = 20;
            
            public static GameObject HealingEffect = Assets.LITAssets.LoadAsset<GameObject>("EffectLeechingBurst");
            
            public static GameObject TracerEffect = Assets.LITAssets.LoadAsset<GameObject>("TracerLeeching");

            public static GameObject AbilityEffect = Assets.LITAssets.LoadAsset<GameObject>("EffectLeechingAbility");

            private GameObject AbilityEffectInstance;

            private List<HealthComponent> healthComponents;

            private SphereSearch healSearch;

            private float healingStopwatch;

            private float abilityStopwatch;

            private bool doingAbility;

            internal void Ability()
            {
                doingAbility = true;
                abilityStopwatch = 0;
                if(!AbilityEffectInstance)
                {
                    AbilityEffectInstance = Instantiate(AbilityEffect, body.transform);
                    if(AbilityEffectInstance)
                    {
                        AbilityEffectInstance.transform.localScale *= body.bestFitRadius;
                    }
                }
            }
            private void Start()
            {
                healSearch = new SphereSearch();
                healthComponents = new List<HealthComponent>();
            }

            private void Update()
            {
                healingStopwatch += Time.deltaTime;
                if (healingStopwatch > timeBetweenHeals)
                {
                    healingStopwatch = 0;
                    HealNearby();
                }
                if(doingAbility)
                {
                    abilityStopwatch += Time.deltaTime;
                    if(abilityStopwatch > abilityDuration)
                    {
                        doingAbility = false;
                        body.RecalculateStats();
                        abilityStopwatch = 0;
                        if (AbilityEffectInstance)
                            Destroy(AbilityEffectInstance);
                    }
                }
            }

            private void HealNearby()
            {
                SearchForAllies();
                bool hasBursted = false;
                healthComponents.Where(hc => hc.body != body)
                    .Where(hc => !hc.body.HasBuff(LeechingRegen.buff))
                    .Where(hc => !hc.body.GetComponent<AffixLeechingBehavior>())
                    .ToList()
                    .ForEach(hc =>
                    {
                        hc.body.AddTimedBuff(LeechingRegen.buff, 5);
                        SpawnFX(TracerEffect, hc.body.corePosition, body.corePosition);
                        if (!hasBursted)
                        {
                            SpawnFX(HealingEffect, body.radius, body.aimOrigin);
                        }
                    });
            }

            private void SearchForAllies()
            {
                List<HurtBox> hurtBoxes = new List<HurtBox>();
                healthComponents.Clear();

                TeamMask mask = default(TeamMask);
                mask.AddTeam(body.teamComponent.teamIndex);
                healSearch.mask = LayerIndex.entityPrecise.mask;
                healSearch.radius = 30;
                healSearch.origin = body.corePosition;
                healSearch.RefreshCandidates();
                healSearch.FilterCandidatesByHurtBoxTeam(mask);
                healSearch.GetHurtBoxes(hurtBoxes);

                hurtBoxes.ForEach(hb =>
                {
                    if (!healthComponents.Contains(hb.healthComponent) && hb.healthComponent.health < hb.healthComponent.fullHealth)
                        healthComponents.Add(hb.healthComponent);
                });
            }


            public void OnDamageDealtServer(DamageReport damageReport)
            {
                if (!doingAbility)
                    damageReport.attackerBody?.healthComponent?.Heal(damageReport.damageDealt * damageReport.damageInfo.procCoefficient, default);
                else
                {
                    SearchForAllies();
                    float healing = (damageReport.damageDealt * 1.5f) / healthComponents.Count;
                    foreach (HealthComponent component in healthComponents)
                    {
                        SpawnFX(TracerEffect, component.body.corePosition, body.corePosition);
                        component.Heal(healing, default);
                    }
                }
            }

            public void OnDestroy()
            {
                if (AbilityEffectInstance)
                    Destroy(AbilityEffectInstance);
            }

            #region Effects
            private void SpawnFX(GameObject Effect, float scale, Vector3 originPosition)
            {
                EffectData effectData = new EffectData
                {
                    scale = scale,
                    origin = originPosition
                };
                EffectManager.SpawnEffect(Effect, effectData, true);
            }

            private void SpawnFX(GameObject Effect, Vector3 origin, Vector3 start)
            {
                EffectData effectData = new EffectData
                {
                    origin = origin,
                    start = start
                };
                EffectManager.SpawnEffect(TracerEffect, effectData, true);
            }
            #endregion
            
        }
    }
}
