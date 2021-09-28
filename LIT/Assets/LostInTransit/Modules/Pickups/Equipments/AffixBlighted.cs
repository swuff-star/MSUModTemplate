using RoR2;
using UnityEngine;
using Moonstorm;
using Moonstorm.Utilities;

namespace LostInTransit.Equipments
{
    public class AffixBlighted : EliteEquipmentBase
    {
        public override MSEliteDef EliteDef { get; set; } = Assets.LITAssets.LoadAsset<MSEliteDef>("Blighted");
        public override EquipmentDef EquipmentDef { get; set; } = Assets.LITAssets.LoadAsset<EquipmentDef>("AffixBlighted");
        public override MSAspectAbilityDataHolder AspectAbilityData { get; set; } = Assets.LITAssets.LoadAsset<MSAspectAbilityDataHolder>("AbilityBlighted");

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<BlightStatIncrease>(stack);
        }

        public override bool FireAction(EquipmentSlot slot)
        {
            if(MSUtil.IsModInstalled("com.TheMysticSword.AspectAbilities"))
            {
                var component = slot.characterBody.GetComponent<Buffs.AffixBlighted.AffixBlightedBehavior>();
                if(component)
                {
                    component.MasterBehavior.Ability();
                    return true;
                }
            }
            return false;
        }

        public class BlightStatIncrease : CharacterBody.ItemBehavior
        {
            public void Start()
            {
                body.baseMaxHealth *= 7.0f;
                body.baseDamage *= 3;
                body.baseMoveSpeed *= 1.1f;
                body.PerformAutoCalculateLevelStats();

                body.healthComponent.health = body.healthComponent.fullHealth;
            }

            public void OnDestroy()
            {
                body.baseMaxHealth /= 7.0f;
                body.baseDamage /= 3f;
                body.baseMoveSpeed /= 1.1f;
                body.PerformAutoCalculateLevelStats();
            }
        }
    }
}
