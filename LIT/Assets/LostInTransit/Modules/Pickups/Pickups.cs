using RoR2;
using System.Collections.Generic;
using System.Linq;
using Moonstorm;
using LostInTransit.Utils;
using RoR2.ContentManagement;
using System.Reflection;

namespace LostInTransit.Modules
{
    public class Pickups : PickupModuleBase
    {
        public static Pickups Instance { get; set; }
        public static ItemDef[] LoadedLITItems { get => LITContent.serializableContentPack.itemDefs; }
        public static EquipmentDef[] LoadedLITEquipments { get => LITContent.serializableContentPack.equipmentDefs; }
        public override SerializableContentPack ContentPack { get; set; } = LITContent.serializableContentPack;

        public override void Init()
        {
            Instance = this;
            base.Init();
            LITLogger.LogI($"Initializing Pickups...");
            if(LITConfig.EnableItems.Value)
            {
                LITLogger.LogD($"Initializing Items...");
                InitializeItems();
            }
            if(LITConfig.EnableEquipments.Value)
            {
                LITLogger.LogD($"Initializing Equipments...");
                InitializeEquipments();
                InitializeEliteEquipments();
            }
        }
        public override IEnumerable<ItemBase> InitializeItems()
        {
            base.InitializeItems()
                .Where(item => LITMain.config.Bind<bool>("Item: " + item.ItemDef.name, "Enable Item", true, "Wether or not to enable this item.").Value)
                .ToList()
                .ForEach(item => AddItem(item, ContentPack));
            return null;
        }
        public override IEnumerable<EquipmentBase> InitializeEquipments()
        {
            base.InitializeEquipments()
                .Where(equip => LITMain.config.Bind<bool>("Equipment: " + equip.EquipmentDef.name, "Enable Equipment", true, "Wether or not to enable this equipment.").Value)
                .ToList()
                .ForEach(equip => AddEquipment(equip, ContentPack));
            return null;
        }

        public override IEnumerable<EliteEquipmentBase> InitializeEliteEquipments()
        {
            base.InitializeEliteEquipments()
                .ToList()
                .ForEach(equip => AddEliteEquipment(equip));
            return null;
        }
    }
}
