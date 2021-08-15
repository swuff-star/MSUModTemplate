using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LostInTransit.Modules;
using System;
using UnityEngine.Networking;

namespace LostInTransit.Components
{
    [RequireComponent(typeof(CharacterBody))]
    public class LITItemManager : MonoBehaviour
    {
        private CharacterBody body;

        IStatItemBehavior[] statItemBehaviors = Array.Empty<IStatItemBehavior>();

        private void Awake()
        {
            body = gameObject.GetComponent<CharacterBody>();
            body.onInventoryChanged += CheckForLITItems;
        }

        public void CheckForLITItems()
        {
            foreach (var item in Pickups.Items)
            {
                item.Value.AddBehavior(ref body, body.inventory.GetItemCount(item.Key.itemIndex));
            }
            /*foreach(var equip in Pickups.Equipments)
            {
                equip.Value.AddBehavior(ref body, Convert.ToInt32(body.inventory?.GetEquipmentIndex() == equip.Value.EquipmentDef.equipmentIndex));
            }*/
            GetInterfaces();
        }

        private void GetInterfaces()
        {
            statItemBehaviors = GetComponents<IStatItemBehavior>();
            if (NetworkServer.active)
            {
                body.healthComponent.onIncomingDamageReceivers = GetComponents<IOnIncomingDamageServerReceiver>();
                body.healthComponent.onTakeDamageReceivers = GetComponents<IOnTakeDamageServerReceiver>();
            }
        }


        public void RunStatRecalculationsStart()
        {
            foreach (var statBehavior in statItemBehaviors)
                statBehavior.RecalcStatsStart();
        }
        public void RunStatRecalculationsEnd()
        {
            foreach (var statBehavior in statItemBehaviors)
                statBehavior.RecalcStatsEnd();
        }
    }
}