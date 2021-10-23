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

        [ConfigurableField(ConfigName = "Bigger Blighted Elites", ConfigDesc = "Makes Blighted Elites bigger, and gives them a health bar. Fun bonus feature that may not be maintained.")]
        public static bool biggerBlighted = false;
        //Just a fun little bonus feature, semi-inspired by Shooty's proposed reworks.

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
                if (biggerBlighted = true)
                //I invoke an ancient evil to do this... through code!
                {
                    CharacterBody body = this.GetComponent<CharacterBody>();
                    if (body)
                    {
                        ModelLocator modelLocator = this.GetComponent<ModelLocator>();
                        if (modelLocator)
                        {
                            Transform modelTransform = modelLocator.modelBaseTransform;
                            if (modelTransform)
                            {
                                modelTransform.localScale *= 1.5f;
                            }

                            HurtBoxGroup hurtbox = modelLocator.modelTransform.GetComponent<HurtBoxGroup>();
                            if (hurtbox)
                            {
                                if (hurtbox.hurtBoxes[0])
                                {
                                    hurtbox.hurtBoxes[0].transform.localScale = new Vector3(1.7f, 1.4f, 1);
                                }
                            }
                        }
                    }
                }
            }

            public void OnDestroy()
            {
                body.baseMaxHealth /= 7.0f;
                body.baseDamage /= 2f;
                body.baseMoveSpeed /= 1.1f;
                body.PerformAutoCalculateLevelStats();

                if (biggerBlighted = true)
                //I invoke an ancient evil to do this... through code!
                {
                    CharacterBody body = this.GetComponent<CharacterBody>();
                    if (body)
                    {
                        ModelLocator modelLocator = this.GetComponent<ModelLocator>();
                        if (modelLocator)
                        {
                            Transform modelTransform = modelLocator.modelBaseTransform;
                            if (modelTransform)
                            {
                                modelTransform.localScale /= 1.5f;
                            }

                            HurtBoxGroup hurtbox = modelLocator.modelTransform.GetComponent<HurtBoxGroup>();
                            if (hurtbox)
                            {
                                if (hurtbox.hurtBoxes[0])
                                {
                                    hurtbox.hurtBoxes[0].transform.localScale = new Vector3(-1.7f, -1.4f, -1);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
