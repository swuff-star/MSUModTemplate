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
    public class AffixLeeching : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("BuffAffixLeeching");
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
            public float timeBetweenHeals = 15;

            public GameObject VisualEffect = Assets.LITAssets.LoadAsset<GameObject>("VFXLeeching");

            private List<HurtBox> hurtBoxes;

            private SphereSearch healSearch;

            private GameObject VFXInstance;

            private float stopwatch;

            public void Awake()
            {
                hurtBoxes = new List<HurtBox>();
                healSearch = new SphereSearch();
                var component = VisualEffect.GetComponent<ScaleByBodyRadius>();
                component.body = body;
            }
            public void Update()
            {
                stopwatch += Time.deltaTime;
                if(stopwatch >= timeBetweenHeals)
                {
                    stopwatch = 0;
                    HealNearby();
                }
            }

            private void HealNearby()
            {
                var hasBursted = false;
                TeamMask mask = default(TeamMask);
                mask.AddTeam(body.teamComponent.teamIndex);
                hurtBoxes.Clear();
                healSearch.mask = LayerIndex.entityPrecise.mask;
                healSearch.radius = 256;
                healSearch.origin = body.corePosition;
                healSearch.RefreshCandidates();
                healSearch.FilterCandidatesByHurtBoxTeam(mask);
                healSearch.GetHurtBoxes(hurtBoxes);
                foreach(HurtBox h in hurtBoxes)
                {
                    if(h.healthComponent.body != body)
                    {
                        h.healthComponent.body.AddTimedBuff(RoR2Content.Buffs.CrocoRegen, 5);
                        if(!hasBursted)
                        {
                            VFXInstance = Instantiate(VisualEffect, body.aimOriginTransform);
                            hasBursted = true;
                        }
                    }

                }
            }
            public void OnDamageDealtServer(DamageReport damageReport)
            {
                damageReport.attackerBody?.healthComponent?.Heal((damageReport.damageDealt * (damageReport.damageInfo.procCoefficient * 0.25f)), default);
            }
        }
    }
}
