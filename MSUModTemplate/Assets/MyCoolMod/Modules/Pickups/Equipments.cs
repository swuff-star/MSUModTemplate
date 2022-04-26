using Moonstorm;
using R2API.ScriptableObjects;
using RoR2;
using System.Collections.Generic;
using System.Linq;

namespace MyMod.Modules
{
    public sealed class Equipments : EquipmentModuleBase
    {
        public static Equipments Instance { get; private set; }
        public static EquipmentDef[] LoadedLITEquipments { get => MyModContent.Instance.SerializableContentPack.equipmentDefs; }
        public override R2APISerializableContentPack SerializableContentPack => MyModContent.Instance.SerializableContentPack;

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            MyModLogger.LogI($"Initializing Equipments...");
            GetEquipmentBases();
            GetEliteEquipmentBases();
        }

        protected override IEnumerable<EquipmentBase> GetEquipmentBases()
        {
            base.GetEquipmentBases()
                .Where(eqp => MyModMain.config.Bind<bool>(eqp.EquipmentDef.name, "Enable Equipment", true, "Whether or not to enable this equipment.").Value)
                .ToList()
                .ForEach(eqp => AddEquipment(eqp));
            return null;
        }

        protected override IEnumerable<EliteEquipmentBase> GetEliteEquipmentBases()
        {
            base.GetEliteEquipmentBases()
                .ToList()
                .ForEach(eeqp => AddEliteEquipment(eeqp));
            return null;
        }
    }
}
