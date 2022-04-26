using Moonstorm;
using R2API.ScriptableObjects;
using RoR2;
using RoR2.ContentManagement;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace MyMod.Modules
{
    public sealed class Elites : EliteModuleBase
    {
        public static Elites Instance { get; set; }
        public static MSEliteDef[] LoadedLITElites { get => MyModContent.Instance.SerializableContentPack.eliteDefs as MSEliteDef[]; }
        public override R2APISerializableContentPack SerializableContentPack => MyModContent.Instance.SerializableContentPack;
        public override AssetBundle AssetBundle => MyModAssets.Instance.MainAssetBundle;

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            MyModLogger.LogI($"Initializing Elites...");
            GetInitializedEliteEquipmentBases();
            OnListCreated += LateEliteSetup;
        }

        protected override IEnumerable<EliteEquipmentBase> GetInitializedEliteEquipmentBases()
        {
            base.GetInitializedEliteEquipmentBases()
                .Where(elite => MyModMain.config.Bind<bool>("My Totally Radical & Epic Elites", elite.EliteDef.name, true, "Enable/disable this Elite Type.").Value)
                .ToList()
                .ForEach(elite => AddElite(elite));
            return null;
        }

        private void LateEliteSetup(ReadOnlyCollection<MSEliteDef> eliteCollection)
        {
        }
    }
}