using LostInTransit.Components;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Equipment = LostInTransit.Equipments.EquipmentBase;
using Item = LostInTransit.Items.ItemBase;
using LostInTransit.Utils;
using LostInTransit.Equipments;

namespace LostInTransit.Modules
{
    internal static class Pickups
    {
        public static ItemDef[] loadedItemDefs
        {
            get
            {
                return LITContent.serializableContentPack.itemDefs;
            }
        }

        public static EquipmentDef[] loadedEquipmentDefs
        {
            get
            {
                return LITContent.serializableContentPack.equipmentDefs;
            }
        }

        public static Dictionary<ItemDef, Item> Items = new Dictionary<ItemDef, Item>();
        public static Dictionary<string, UnityEngine.Object> ItemPickups = new Dictionary<string, UnityEngine.Object>();

        public static Dictionary<EquipmentDef, Equipment> Equipments = new Dictionary<EquipmentDef, Equipment>();
        public static Dictionary<string, UnityEngine.Object> EquipmentPickups = new Dictionary<string, UnityEngine.Object>();

        public static Dictionary<EquipmentDef, EliteEquipment> EliteEquipments = new Dictionary<EquipmentDef, EliteEquipment>();

        public static void Initialize()
        {
            LITLogger.LogI("Initializing Pickups");
            if (LITConfig.EnableItems.Value)
            {
                LITLogger.LogD("Initializing Items...");
                InitializeItems();
            }
            if(LITConfig.EnableEquipments.Value)
            {
                LITLogger.LogD("Initializing Equipments...");
                InitializeEquipments();
                InitializeEliteEquipments();
            }
            On.RoR2.EquipmentSlot.PerformEquipmentAction += FireLITEqp;
            CharacterBody.onBodyStartGlobal += AddItemManager;
            On.RoR2.CharacterBody.RecalculateStats += OnRecalcStats;
        }


        private static void InitializeItems()
        {
            typeof(Pickups).Assembly.GetTypes()
                .Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(Item)))
                .Select(itemType => (Item)Activator.CreateInstance(itemType))
                .ToList()
                .ForEach(item =>
                {
                    HG.ArrayUtils.ArrayAppend(ref LITContent.serializableContentPack.itemDefs, item.ItemDef);
                    item.Initialize();
                    Items.Add(item.ItemDef, item);
                    LITLogger.LogD($"Added item {item.ItemDef.name}");
                });
        }

        private static void InitializeEquipments()
        {
            typeof(Pickups).Assembly.GetTypes()
                .Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(Equipment)) && !type.IsSubclassOf(typeof(EliteEquipment)))
                .Select(eqpType => (Equipment)Activator.CreateInstance(eqpType))
                .ToList()
                .ForEach(equipment =>
                {
                    HG.ArrayUtils.ArrayAppend(ref LITContent.serializableContentPack.equipmentDefs, equipment.EquipmentDef);
                    equipment.Initialize();
                    Equipments.Add(equipment.EquipmentDef, equipment);
                    LITLogger.LogD($"Added equipment {equipment.EquipmentDef.name}");
                });
        }

        private static void InitializeEliteEquipments()
        {
            typeof(Pickups).Assembly.GetTypes()
                .Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(EliteEquipment)))
                .Select(eqpEliteType => (EliteEquipment)Activator.CreateInstance(eqpEliteType))
                .ToList()
                .ForEach(equipElite =>
                {
                    EliteEquipments.Add(equipElite.EquipmentDef, equipElite);
                    LITLogger.LogD($"Added Equipment Elite {equipElite.EquipmentDef.name}");
                });
        }

        private static void AddItemManager(CharacterBody body)
        {
            if(!body.bodyFlags.HasFlag(CharacterBody.BodyFlags.Masterless) && body.master.inventory)
            {
                var itemManager = body.gameObject.AddComponent<LITItemManager>();
                itemManager.CheckForLITItems();
            }
        }

        //Kevin moment
        private static void OnRecalcStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            var manager = self.GetComponent<LITItemManager>();
            manager?.RunStatRecalculationsStart();
            orig(self);
            manager?.RunStatRecalculationsEnd();
        }
        //Keving moment 2
        private static bool FireLITEqp(On.RoR2.EquipmentSlot.orig_PerformEquipmentAction orig, EquipmentSlot self, EquipmentDef equipmentDef)
        {
            if(!NetworkServer.active)
            {
                Debug.LogWarning("[Server] function 'System.Boolean RoR2.EquipmentSlot::PerformEquipmentAction(RoR2.EquipmentDef)' called on client");
                return false;
            }
            Equipment equipment;
            if(Equipments.TryGetValue(equipmentDef, out equipment))
            {
                var body = self.characterBody;
                return equipment.FireAction(self);
            }
            return orig(self, equipmentDef);
        }

    }
}
