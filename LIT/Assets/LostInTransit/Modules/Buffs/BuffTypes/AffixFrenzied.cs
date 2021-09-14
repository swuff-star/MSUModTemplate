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
            public GameObject BlinkReadyEffect = null;

            private bool blinkReady = true;

            internal bool doingAbility = false;
            
            private float blinkTimer = 0;

            private float abilityStopwatch = 0;

            private int multiplier = 1;

            private void Start()
            {
                body.RecalculateStats();
            }
            public void Update()
            {
                UpdateTimers();
                //We DO NOT want players to be blinked outside of their control. thats just a bad idea.
                if(body.isPlayerControlled)
                {
                    if (Input.GetKeyDown(LITConfig.FrenziedBlink.Value) && blinkReady)
                    {
                        var bodyStateMachine = body.GetComponents<EntityStateMachine>().Where(x => x.customName == "Body").FirstOrDefault();
                        if (bodyStateMachine)
                            Blink(bodyStateMachine);
                    }
                }
                else
                {
                    if(blinkReady)
                    {
                        var bodyStateMachine = body.GetComponents<EntityStateMachine>().Where(x => x.customName == "Body").FirstOrDefault();
                        if (bodyStateMachine)
                            Blink(bodyStateMachine);
                    }

                }

            }

            private void UpdateTimers()
            {
                if(body.healthComponent.alive)
                {
                    blinkTimer += (Time.deltaTime * multiplier);
                    if (blinkTimer >= 10)
                    {
                        blinkReady = true;
                    }
                    if(doingAbility)
                    {
                        abilityStopwatch += Time.deltaTime;
                        if(abilityStopwatch >= 10)
                        {
                            doingAbility = false;
                            multiplier = 1;
                            abilityStopwatch = 0;
                        }
                    }
                }
            }

            internal void Ability()
            {
                doingAbility = true;
                multiplier = 2;
                body.RecalculateStats();
            }

            private void Blink(EntityStateMachine bodyStateMachine)
            {
                if(body.healthComponent.alive)
                {
                    //Todd Howard Voice: It just works.
                    //ToDo: Probably make this into a proper entity state instead of re-using the parent's.
                    bodyStateMachine.SetNextState(new EntityStates.ParentMonster.LoomingPresence());
                    blinkTimer = 0;
                    blinkReady = false;
                }
            }

            public void RecalculateStatsEnd()
            {
                body.moveSpeed *= 2f;
                body.attackSpeed *= 2f;

                var cooldownModifier = 0.0f;
                if (doingAbility)
                {
                    cooldownModifier = 2.5f;
                }

                if (body.skillLocator.primary)
                    body.skillLocator.primary.cooldownScale -= 0.5f + cooldownModifier;
                if (body.skillLocator.secondary)
                    body.skillLocator.secondary.cooldownScale -= 0.5f + cooldownModifier;
                if (body.skillLocator.utility)
                    body.skillLocator.utility.cooldownScale -= 0.5f + cooldownModifier;
                if (body.skillLocator.special)
                    body.skillLocator.special.cooldownScale -= 0.5f + cooldownModifier;
            }

            public void RecalculateStatsStart()
            {

            }
        }
    }
}
