using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LostInTransit.ScriptableObjects;
using LostInTransit.Modules;
using LostInTransit.Components;
using AspectAbilities;

namespace LostInTransit.Equipments
{
    public abstract class EliteEquipment : EquipmentBase
    {
        public abstract LITEliteDef EliteDef { get; set; }

        public abstract LITAspectAbility AspectAbility { get; set; }

        public override void Initialize()
        {
            if(LITMain.AspectAbilitiesInstalled)
            {
                RunAspectAbility();
            }
        }

        private void RunAspectAbility()
        {
            AspectAbility ability = AspectAbility.CreateAbility();
            ability.onUseOverride = FireAction;
            AspectAbilitiesPlugin.RegisterAspectAbility(ability);
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            var behavior = body.AddItemBehavior<EliteBehavior>(stack);
            if (behavior)
                behavior.elite = EquipmentDef.passiveBuffDef.eliteDef as LITEliteDef;
        }

        public class EliteBehavior : CharacterBody.ItemBehavior
        {
            public LITEliteDef elite;

            private CharacterModel model;
            private GameObject effectInstance;

            public void Start()
            {
                if(Buffs.Buffs.buffs.TryGetValue(elite.eliteEquipmentDef.passiveBuffDef, out var buffBase))
                {
                    buffBase.AddBehavior(ref body, stack);
                }
                model = body.modelLocator.modelTransform.GetComponent<CharacterModel>();
                if(!model)
                {
                    Destroy(this);
                    return;
                }
                if (elite.effect)
                {
                    var scaleComponent = elite.effect.GetComponent<ScaleByBodyRadius>();
                    if((bool)scaleComponent)
                    {
                        scaleComponent.body = body;
                    }
                    effectInstance = Instantiate(elite.effect, body.aimOriginTransform, false);
                }
            }

            public void OnDestroy()
            {
                Debug.Log("A");
                model.propertyStorage.SetTexture(Elites.EliteRampPropertyID, Shader.GetGlobalTexture(Elites.EliteRampPropertyID));
                Debug.Log("A");
                if (effectInstance)
                {
                    Debug.Log("A");
                    Destroy(effectInstance);
                    Debug.Log("A");
                }
            }
        }
    }
}
