using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LostInTransit.ScriptableObjects;
using LostInTransit.Modules;

namespace LostInTransit.Equipments
{
    public abstract class EliteEquipment : EquipmentBase
    {
        public abstract LITEliteDef EliteDef { get; set; }

        public override void Initialize()
        {
            base.Initialize();
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
                model = body.modelLocator.modelTransform.GetComponent<CharacterModel>();
                if(!model)
                {
                    Destroy(this);
                    return;
                }
                if (elite.effect)
                    effectInstance = Instantiate(elite.effect, body.aimOriginTransform, false);
            }

            public void OnDestroy()
            {
                model.propertyStorage.SetTexture(Elites.EliteRampPropertyID, Shader.GetGlobalTexture(Elites.EliteRampPropertyID));
                if (effectInstance)
                    Destroy(effectInstance);
            }
        }
    }
}
