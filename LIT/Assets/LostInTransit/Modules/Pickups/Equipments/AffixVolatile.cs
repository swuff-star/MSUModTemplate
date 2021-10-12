using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonstorm;
using RoR2;
using Moonstorm.Utilities;
using UnityEngine.Networking;

namespace LostInTransit.Equipments
{
    public class AffixVolatile : EliteEquipmentBase
    {
        public override MSEliteDef EliteDef { get; set; } = Assets.LITAssets.LoadAsset<MSEliteDef>("Volatile");
        public override MSAspectAbilityDataHolder AspectAbilityData { get; set; } = Assets.LITAssets.LoadAsset<MSAspectAbilityDataHolder>("AbilityVolatile");
        public override EquipmentDef EquipmentDef { get; set; } = Assets.LITAssets.LoadAsset<EquipmentDef>("AffixVolatile");

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            if (MSUtil.IsModInstalled("com.TheMysticSword.AspectAbilities"))
            {
                if(NetworkServer.active)
                {
                    body.AddItemBehavior<AffixVolatileEquipBehavior>(stack);
                }
            }
        }
        public override bool FireAction(EquipmentSlot slot)
        {
            if (MSUtil.IsModInstalled("com.TheMysticSword.AspectAbilities"))
            {
                var component = slot.characterBody.GetComponent<AffixVolatileEquipBehavior>();
                if (component)
                {
                    component.Explode();
                    return true;
                }
            }
            return false;
        }

        public class AffixVolatileEquipBehavior : CharacterBody.ItemBehavior
        {
            public static GameObject volatileAttachment = Assets.LITAssets.LoadAsset<GameObject>("VolatileEquipBodyAttachment");
            private NetworkedBodyAttachment attachment;
            private EntityStateMachine entityStateMachine;

            private void Start()
            {
                attachment = Object.Instantiate(volatileAttachment).GetComponent<NetworkedBodyAttachment>();
                attachment.AttachToGameObjectAndSpawn(body.gameObject);
                entityStateMachine = attachment.gameObject.GetComponent<EntityStateMachine>();
            }

            public void Explode()
            {
                if (entityStateMachine)
                {
                    entityStateMachine.SetNextState(new EntityStates.Elites.VolatileExplosion());
                }
            }

            private void OnDestroy()
            {
                if((bool)attachment)
                {
                    Object.Destroy(attachment.gameObject);
                    attachment = null;
                }
            }
        }
    }
}
