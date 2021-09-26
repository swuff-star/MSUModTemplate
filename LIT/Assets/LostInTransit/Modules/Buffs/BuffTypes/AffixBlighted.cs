using Moonstorm;
using RoR2;
using System.Collections.Generic;
using R2API.Utils;
using UnityEngine.Networking;
using LostInTransit.Components;
using UnityEngine;
using Moonstorm;

namespace LostInTransit.Buffs
{
    public class AffixBlighted : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("AffixBlighted");

        public static BuffDef buffDef;

        public override void Initialize()
        {
            buffDef = BuffDef;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<AffixBlightedBehavior>(stack);
        }
        public class AffixBlightedBehavior : CharacterBody.ItemBehavior, IStatItemBehavior
        {

            public BlightedController MasterBehavior { get => body.masterObject?.GetComponent<BlightedController>(); }

            public BuffDef firstBuff;

            public BuffDef secondBuff;

            private float stopwatch;
            private static float checkTimer = 0.5f;

            public void Start()
            {
                MasterBehavior.enabled = true;
                MasterBehavior.buffBehavior = this;

                body.onSkillActivatedServer += RemoveBuff;
            }

            private void RemoveBuff(GenericSkill obj)
            {
                if (body.hasCloakBuff)
                    body.RemoveBuff(RoR2Content.Buffs.Cloak);
            }

            public void SetElites(int index1, int index2)
            {
                EliteDef firstElite = EliteCatalog.GetEliteDef((EliteIndex)index1);
                EliteDef secondElite = EliteCatalog.GetEliteDef((EliteIndex)index2);

                //Dont replace the buff if theyre a match.
                if (firstBuff != firstElite.eliteEquipmentDef.passiveBuffDef)
                {
                    body.RemoveBuff(firstBuff);
                    firstBuff = firstElite.eliteEquipmentDef.passiveBuffDef;
                    body.AddBuff(firstBuff);
                }

                if (secondBuff != secondElite.eliteEquipmentDef.passiveBuffDef)
                {
                    body.RemoveBuff(secondBuff);
                    secondBuff = secondElite.eliteEquipmentDef.passiveBuffDef;
                    body.AddBuff(secondBuff);
                }
            }

            public void FixedUpdate()
            {
                stopwatch += Time.fixedDeltaTime;
                if (stopwatch > checkTimer && !body.HasBuff(RoR2Content.Buffs.Cloak))
                {
                    stopwatch = 0;
                    body.AddBuff(RoR2Content.Buffs.Cloak);
                }
                else if (Util.CheckRoll(1))
                    body.RemoveBuff(RoR2Content.Buffs.Cloak);
                /*if (body.skillLocator.primary.cooldownRemaining > 0 ^ body.skillLocator.secondary.cooldownRemaining > 0 ^ body.skillLocator.utility.cooldownRemaining > 0 ^ body.skillLocator.special.cooldownRemaining > 0 ^ Util.CheckRoll(1))
                    { body.RemoveBuff(RoR2Content.Buffs.Cloak.buffIndex); }*/
                //This is entirely untested but SHOULD work. We're all about untested code here, right?
            }

            public void RecalculateStatsStart() { }

            public void RecalculateStatsEnd()
            {
                //Reduces cooldowns by 50%
                if (body.skillLocator.primary)
                    body.skillLocator.primary.cooldownScale *= 0.5f;
                if (body.skillLocator.secondary)
                    body.skillLocator.secondary.cooldownScale *= 0.5f;
                if (body.skillLocator.utility)
                    body.skillLocator.utility.cooldownScale *= 0.5f;
                if (body.skillLocator.special)
                    body.skillLocator.special.cooldownScale *= 0.5f;
                //Is there a reason you subtract CDR instead of multiply? If two things gave 0.5 CDR like this, then it'd have 0 CDR... Right?
                //Also, if these need nerfed, I say this is the first thing to go.
                //Neb: i supose i can change it to multiply by 0.5 if they need nerfed. we'll see in the future.
            }

            public void OnDestroy()
            {
                MasterBehavior.enabled = false;
                body.RemoveBuff(firstBuff);
                body.RemoveBuff(secondBuff);

                body.onSkillActivatedServer -= RemoveBuff;

                body.RemoveBuff(RoR2Content.Buffs.Cloak);
            }
        }
    }
}