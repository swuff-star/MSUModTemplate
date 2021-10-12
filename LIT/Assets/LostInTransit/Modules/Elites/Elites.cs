using LostInTransit.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using Moonstorm;
using RoR2.ContentManagement;
using System.Reflection;
using System.Linq;
using LostInTransit.Components;
using UnityEngine.Networking;
using LostInTransit.Elites;

namespace LostInTransit.Modules
{
    public class Elites : EliteModuleBase
    {
        public static Elites Instance { get; set; }
        public static EliteDef[] LoadedLITElites { get => LITContent.serializableContentPack.eliteDefs; }
        public static MSEliteDef[] LoadedLITElitesAsMSElites { get => LITContent.serializableContentPack.eliteDefs as MSEliteDef[]; }
        public override SerializableContentPack ContentPack { get; set; } = LITContent.serializableContentPack;
        public override AssetBundle AssetBundle { get; set; } = Assets.LITAssets;

        private static bool spawnedDirector = false;

        public override void Init()
        {
            Instance = this;
            base.Init();
            LITLogger.LogI($"Initializing Elites...");
            InitializeEliteEquipments();
        }

        public override IEnumerable<EliteEquipmentBase> InitializeEliteEquipments()
        {
            base.InitializeEliteEquipments()
                .Where(elite => LITMain.config.Bind<bool>("Lost in Transit Elites", elite.EliteDef.name, true, "Enable/disable this Elite Type.").Value)
                .ToList()
                .ForEach(elite => AddElite(elite, ContentPack));
            LateEliteSetup();
            return null;
        }

        private void LateEliteSetup()
        {
            if(MoonstormElites.Contains(Assets.LITAssets.LoadAsset<MSEliteDef>("Volatile")))
            {
                VolatileSpitebomb.BeginSetup();
            }
            if(MoonstormElites.Contains(Assets.LITAssets.LoadAsset<MSEliteDef>("Blighted")))
            {
                Blight.BeginSetup();
            }
        }
    }
}