using Moonstorm;
using R2API.ScriptableObjects;
using RoR2;
using System.Collections.Generic;
using System.Linq;

namespace MyMod.Modules
{
    public sealed class Items : ItemModuleBase
    {
        public static Items Instance { get; private set; }
        public static ItemDef[] LoadedLITItems { get => MyModContent.Instance.SerializableContentPack.itemDefs; }
        public override R2APISerializableContentPack SerializableContentPack => MyModContent.Instance.SerializableContentPack;

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            MyModLogger.LogI($"Initializing Items...");
            GetItemBases();
        }

        protected override IEnumerable<ItemBase> GetItemBases()
        {
            base.GetItemBases()
                .Where(item => MyModMain.config.Bind<bool>(item.ItemDef.name, "Enable Item", true, "Whether or not to enable this item.").Value)
                .ToList()
                .ForEach(item => AddItem(item, null));
            return null;
        }
    }
}
