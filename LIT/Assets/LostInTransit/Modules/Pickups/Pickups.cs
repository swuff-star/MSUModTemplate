using Moonstorm;
using RoR2;
using RoR2.ContentManagement;
using System.Collections.Generic;
using System.Linq;

namespace LostInTransit.Modules
{
    public class Pickups : PickupModuleBase
    {
        public static Pickups Instance { get; set; }
        public static ItemDef[] LoadedLITItems { get => LITContent.Instance.SerializableContentPack.itemDefs; }
        public static EquipmentDef[] LoadedLITEquipments { get => LITContent.Instance.SerializableContentPack.equipmentDefs; }
        public override SerializableContentPack ContentPack { get; set; } = LITContent.Instance.SerializableContentPack;

        public override void Init()
        {
            Instance = this;
            base.Init();
            LITLogger.LogI($"Initializing Pickups...");
            if (LITConfig.EnableItems.Value)
            {
                LITLogger.LogD($"Initializing Items...");
                InitializeItems();
            }
            if (LITConfig.EnableEquipments.Value)
            {
                LITLogger.LogD($"Initializing Equipments...");
                InitializeEquipments();
                InitializeEliteEquipments();
            }
        }
        public override IEnumerable<ItemBase> InitializeItems()
        {
            base.InitializeItems()
                .Where(item => LITMain.config.Bind<bool>(item.ItemDef.name, "Enable Item", true, "Wether or not to enable this item.").Value)
                .ToList()
                .ForEach(item => AddItem(item, ContentPack));
            return null;
        }
        public override IEnumerable<EquipmentBase> InitializeEquipments()
        {
            base.InitializeEquipments()
                .Where(equip => LITMain.config.Bind<bool>(equip.EquipmentDef.name, "Enable Equipment", true, "Wether or not to enable this equipment.").Value)
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
