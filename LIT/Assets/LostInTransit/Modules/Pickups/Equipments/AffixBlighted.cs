using KinematicCharacterController;
using Moonstorm;
using RoR2;
using UnityEngine;

namespace LostInTransit.Equipments
{
    public class AffixBlighted : EliteEquipmentBase
    {
        public override MSEliteDef EliteDef { get; set; } = Assets.LITAssets.LoadAsset<MSEliteDef>("Blighted");
        public override EquipmentDef EquipmentDef { get; set; } = Assets.LITAssets.LoadAsset<EquipmentDef>("AffixBlighted");
        public override MSAspectAbilityDataHolder AspectAbilityData { get; set; } = Assets.LITAssets.LoadAsset<MSAspectAbilityDataHolder>("AbilityBlighted");

        [ConfigurableField(ConfigName = "Boss Blighted", ConfigDesc = "Whether Teleporter Bosses should spawn as Blighted enemies.")]
        public static bool bossBlighted = false;

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<BlightStatIncrease>(stack);
        }

        public override bool FireAction(EquipmentSlot slot)
        {
            if (MSUtil.IsModInstalled("com.TheMysticSword.AspectAbilities"))
            {
                var component = slot.characterBody.GetComponent<Buffs.AffixBlighted.AffixBlightedBehavior>();
                if (component)
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
                body.baseDamage *= 2;
                body.baseMoveSpeed *= 1.1f;
                body.PerformAutoCalculateLevelStats();

                body.healthComponent.health = body.healthComponent.fullHealth;

                Util.PlaySound("BlightAppear", body.gameObject);
                
            }


            public void OnDestroy()
            {
                if(body.healthComponent.alive)
                {
                    body.baseMaxHealth /= 7.0f;
                    body.baseDamage /= 2f;
                    body.baseMoveSpeed /= 1.1f;
                    body.PerformAutoCalculateLevelStats();
                }
            }
        }
    }
}
