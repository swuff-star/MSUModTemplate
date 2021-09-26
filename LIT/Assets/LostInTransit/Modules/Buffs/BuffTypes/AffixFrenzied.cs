using RoR2;
using System.Collections.Generic;
using Moonstorm;
using UnityEngine;
using System.Linq;
using EntityStates;

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
        }

        public class AffixFrenziedBehavior : CharacterBody.ItemBehavior, IStatItemBehavior
        {
            public static float blinkCooldown;

            public GameObject BlinkReadyEffect = Assets.LITAssets.LoadAsset<GameObject>("EffectFrenziedTPReady");

            public GameObject AbilityEffect = Assets.LITAssets.LoadAsset<GameObject>("EffectFrenziedAbility");

            private GameObject BlinkReadyInstance;

            private GameObject AbilityInstance;

            private bool blinkReady = false;

            private bool doingAbility = false;

            private float blinkStopwatch;

            private float abilityStopwatch;

            private float cdrMult = 1;

            private void Start()
            {
                blinkCooldown = 10;
                body.RecalculateStats();
            }

            private void Update()
            {
                blinkStopwatch += Time.deltaTime;
                if(blinkStopwatch > blinkCooldown / cdrMult)
                {
                    blinkReady = true;
                    if(!BlinkReadyInstance)
                    {
                        BlinkReadyInstance = Instantiate(BlinkReadyEffect, body.aimOriginTransform);
                        if (BlinkReadyInstance)
                            BlinkReadyInstance.transform.localScale *= body.radius;
                    }
                }
                if (doingAbility)
                {
                    abilityStopwatch += Time.deltaTime;
                    if (abilityStopwatch >= 10)
                    {
                        doingAbility = false;
                        cdrMult = 1;
                        abilityStopwatch = 0;
                        body.RecalculateStats();
                        if (AbilityInstance)
                            Destroy(AbilityInstance);
                    }
                }
                if (blinkReady && body.isPlayerControlled && Input.GetKeyDown(LITConfig.FrenziedBlink.Value))
                    Blink();
                else if(blinkReady && !body.isPlayerControlled)
                    Blink();
            }

            private void Blink()
            {
                if (BlinkReadyInstance)
                    Destroy(BlinkReadyInstance);
                var bodyStateMachine = body.GetComponents<EntityStateMachine>().Where(x => x.customName == "Body").FirstOrDefault();
                if (body.healthComponent.alive && bodyStateMachine)
                {
                    //Todd Howard Voice: It just works.
                    bodyStateMachine.SetNextState(new EntityStates.Elites.FrenziedBlink());
                    blinkStopwatch = 0;
                    blinkReady = false;
                }
            }

            internal void Ability()
            {
                doingAbility = true;
                cdrMult = 2;
                AbilityInstance = Instantiate(AbilityEffect, body.aimOriginTransform);
                if (AbilityInstance)
                    AbilityInstance.transform.localScale *= body.bestFitRadius;
                body.RecalculateStats();
            }

            public void RecalculateStatsStart() { }
            public void RecalculateStatsEnd()
            {
                body.moveSpeed *= 2f;
                body.attackSpeed *= 2f;

                //Ability Innactive = 0.5f, 50% cdr
                //Ability Active = 0.75f, 75% cdr
                var cooldownModifier = 0.5f - (0.5f / cdrMult * (cdrMult - 1));

                if (body.skillLocator.primary)
                    body.skillLocator.primary.cooldownScale *= cooldownModifier;
                if (body.skillLocator.secondary)
                    body.skillLocator.secondary.cooldownScale *= cooldownModifier;
                if (body.skillLocator.utility)
                    body.skillLocator.utility.cooldownScale *= cooldownModifier;
                if (body.skillLocator.special)
                    body.skillLocator.special.cooldownScale *= cooldownModifier;
            }

            private void OnDestroy()
            {
                if (BlinkReadyInstance)
                    Destroy(BlinkReadyInstance);
                if (AbilityInstance)
                    Destroy(AbilityInstance);
            }
        }
    }
}
