using Moonstorm;
using RoR2;
using UnityEngine.Networking;
using LostInTransit.Components;

namespace LostInTransit.Buffs
{
    [DisabledContent]
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

            public BlightedController MasterBehavior { get => body.masterObject.GetComponent<BlightedController>(); }

            public BuffDef firstBuff;

            public BuffDef secondBuff;

            public void Start()
            {
                MasterBehavior.enabled = true;
                MasterBehavior.buffBehavior = this;

                body.baseMaxHealth *= 7.0f;
                body.baseDamage *= 3.2f;
                body.baseMoveSpeed *= 1.1f;
                body.PerformAutoCalculateLevelStats();

                body.healthComponent.health = body.healthComponent.fullHealth;
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

            public void RecalculateStatsStart() { }

            public void RecalculateStatsEnd()
            {
                //Reduces cooldowns by 50%
                if (body.skillLocator.primary)
                    body.skillLocator.primary.cooldownScale -= 0.5f;
                if (body.skillLocator.secondary)
                    body.skillLocator.secondary.cooldownScale -= 0.5f;
                if (body.skillLocator.utility)
                    body.skillLocator.utility.cooldownScale -= 0.5f;
                if (body.skillLocator.special)
                    body.skillLocator.special.cooldownScale -= 0.5f;
            }

            public void OnDestroy()
            {
                MasterBehavior.enabled = false;
                body.RemoveBuff(firstBuff);
                body.RemoveBuff(secondBuff);
                body.baseMaxHealth /= 7.0f;
                body.baseDamage /= 3.2f;
                body.baseMoveSpeed /= 1.1f;
                body.PerformAutoCalculateLevelStats();
            }
        }
    }
}