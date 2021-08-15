using LostInTransit.Components;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Equipment = LostInTransit.Equipments.Equipment;
using Item = LostInTransit.Items.ItemBase;

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

        /*public static EquipmentDef[] loadedEquipmentDefs
        {
            get
            {
                return LITContent.serializableContentPack.equipmentDefs;
            }
        }*/

        public static Dictionary<ItemDef, Item> Items = new Dictionary<ItemDef, Item>();

        public static Dictionary<string, UnityEngine.Object> ItemPickups = new Dictionary<string, UnityEngine.Object>();

        public static void Initialize()
        {
            LITLogger.LogI("Initializing Pickups");
            if (LITConfig.EnableItems.Value)
            {
                LITLogger.LogD("Initializing Items...");
                InitializeItems();
            }

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

    }
}
