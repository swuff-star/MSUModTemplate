using RoR2;
using UnityEngine;

namespace EntityStates.Elites
{
    public class VolatileExplosion : BaseBodyAttachmentState
    {
        public static float baseDuration;
        public static GameObject chargingEffectPrefab;
        public static GameObject explosionEffectPrefab;
        public static string chargingSoundString;
        public static string explosionSoundString;
        public static float explosionDamageCoef;
        public static float baseExplosionRadius;
        public static float explosionForce;

        private bool hasExploded;
        private float duration;
        private float radius;
        private float stopwatch;
        private GameObject chargeEffectInstance;
        private uint soundID;
        private Transform modelTransform;

        public override void OnEnter()
        {
            base.OnEnter();
            if (!attachedBody) return;
            modelTransform = attachedBody?.modelLocator.modelTransform;
            if (!modelTransform) return;

            stopwatch = 0f;
            duration = (baseDuration / attachedBody.attackSpeed) * attachedBody.radius;
            radius = baseExplosionRadius * attachedBody.radius;
            soundID = Util.PlaySound(chargingSoundString, gameObject);
            if((bool)chargingEffectPrefab)
            {
                chargeEffectInstance = Object.Instantiate(chargingEffectPrefab, modelTransform.position, modelTransform.rotation);
                if(chargeEffectInstance)
                {
                    chargeEffectInstance.transform.parent = attachedBody.transform;
                    chargeEffectInstance.transform.localScale *= radius * 2.5f;
                    var component1 = chargeEffectInstance.GetComponent<ScaleParticleSystemDuration>();
                    var component2 = chargeEffectInstance.GetComponentInChildren<LightIntensityCurve>();
                    if(component1)
                        component1.newDuration = duration;
                    if(component2)
                    {
                        component2.timeMax = duration;
                        component2.maxIntensity *= radius;
                    }
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            if(stopwatch >= duration && base.isAuthority && !hasExploded)
            {
                Detonate();
                outer.SetNextStateToMain();
            }
        }

        public void Detonate()
        {

            hasExploded = true;
            Util.PlaySound(explosionSoundString, gameObject);
            if((bool)chargeEffectInstance)
            {
                Destroy(chargeEffectInstance);
            }
            if((bool)explosionEffectPrefab)
            {
                EffectManager.SpawnEffect(explosionEffectPrefab, new EffectData
                {
                    origin = base.transform.position,
                    scale = radius,
                }, transmit: true);
            }

            EffectManager.SpawnEffect(Resources.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXQuick"), new EffectData
            {
                origin = attachedBody.transform.position,
                scale = radius,
                rotation = Util.QuaternionSafeLookRotation(Vector3.one)
            }, transmit: true);

            BlastAttack blastAttack = new BlastAttack();
            blastAttack.attacker = attachedBody.gameObject;
            blastAttack.inflictor = attachedBody.gameObject;
            blastAttack.teamIndex = attachedBody.teamComponent.teamIndex;
            blastAttack.baseDamage = attachedBody.damage * explosionDamageCoef;
            blastAttack.baseForce = explosionForce * attachedBody.radius;
            blastAttack.position = modelTransform.position;
            blastAttack.radius = radius;
            blastAttack.procCoefficient = 2f;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHit;
            blastAttack.Fire();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }

        public override void OnExit()
        {
            base.OnExit();
            //AKSoundEngine.StopPlayingID(soundID);
            if((bool)chargeEffectInstance)
            {
                EntityState.Destroy(chargeEffectInstance);
            }
        }
    }
}
