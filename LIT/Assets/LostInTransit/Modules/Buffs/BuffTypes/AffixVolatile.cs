using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moonstorm;
using RoR2;
using UnityEngine;
using R2API;

namespace LostInTransit.Buffs
{
    public class AffixVolatile : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("AffixVolatile");

        public override void Initialize()
        {
            On.RoR2.GlobalEventManager.OnHitAll += VolatileExplosion;
        }

        private void VolatileExplosion(On.RoR2.GlobalEventManager.orig_OnHitAll orig, GlobalEventManager self, DamageInfo damageInfo, GameObject hitObject)
        {
            orig(self, damageInfo, hitObject);
            var component = damageInfo.attacker?.GetComponent<AffixVolatileBehavior>();
            if(component)
                component.OnHitAll(damageInfo);
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<AffixVolatileBehavior>(stack);
        }

        public class AffixVolatileBehavior : CharacterBody.ItemBehavior, IOnTakeDamageServerReceiver
        {
            private void Start()
            {
                if (body.healthComponent)
                {
                    HG.ArrayUtils.ArrayAppend(ref body.healthComponent.onTakeDamageReceivers, this);
                }
            }
            internal void Ability()
            {

            }

            public void OnTakeDamageServer(DamageReport damageReport)
            {
                Debug.Log("On take Damage!");
            }


            public void OnHitAll(DamageInfo dmgInfo)
            {
                var atkBody = dmgInfo.attacker.GetComponent<CharacterBody>();
                if (dmgInfo.procCoefficient != 0f && !DamageAPI.HasModdedDamageType(dmgInfo, DamageTypes.Volatile.volatileDamageType))
                {
                    float radius = 1.5f + (2.5f * dmgInfo.procCoefficient);
                    float dmgCoef = 0.75f;
                    float baseDamage = Util.OnHitProcDamage(dmgInfo.damage, atkBody.damage, dmgCoef);
                    EffectManager.SpawnEffect(Resources.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXQuick"), new EffectData
                    {
                        origin = dmgInfo.position,
                        scale = radius,
                        rotation = Util.QuaternionSafeLookRotation(dmgInfo.force)
                    }, transmit: true);

                    BlastAttack atk = new BlastAttack
                    {
                        position = dmgInfo.position,
                        baseDamage = baseDamage,
                        baseForce = 0f,
                        radius = radius,
                        attacker = dmgInfo.attacker,
                        inflictor = null
                    };
                    atk.teamIndex = TeamComponent.GetObjectTeam(dmgInfo.attacker);
                    atk.crit = dmgInfo.crit;
                    atk.procChainMask = dmgInfo.procChainMask;
                    atk.damageColorIndex = DamageColorIndex.WeakPoint;
                    atk.falloffModel = BlastAttack.FalloffModel.None;
                    atk.damageType = dmgInfo.damageType;
                    DamageAPI.AddModdedDamageType(atk, DamageTypes.Volatile.volatileDamageType);
                    atk.Fire();
                }
            }

            private void OnDestroy()
            {
                //This SHOULDNT cause any errors because nothing should be fucking with the order of things in this list... I hope.
                if (body.healthComponent)
                {
                    int i = Array.IndexOf(body.healthComponent.onIncomingDamageReceivers, this);
                    if (i > -1)
                    {
                        HG.ArrayUtils.ArrayRemoveAtAndResize(ref body.healthComponent.onIncomingDamageReceivers, body.healthComponent.onIncomingDamageReceivers.Length, i);
                    }
                }
            }
        }
    }
}
