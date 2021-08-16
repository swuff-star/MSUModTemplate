using BepInEx;
using BepInEx.Configuration;
using LostInTransit.Modules;
using R2API.Utils;
using R2API;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using LostInTransit.Utils;
using LostInTransit.Buffs;

#pragma warning disable CS0618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618
[module: UnverifiableCode]

namespace LostInTransit
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        nameof(DotAPI)       
    })]
    public class LITMain : BaseUnityPlugin
    {
        internal const string GUID = "com.Swuff.LostInTransit";
        internal const string MODNAME = "Lost in Transit";
        internal const string VERSION = "0.0.1";

        public static LITMain instance;

        public static PluginInfo pluginInfo;

        public static ConfigFile config;

        public static bool DEBUG = true;

        public void Awake()
        {
            instance = this;

            pluginInfo = Info;

            config = Config;

            LITLogger.logger = Logger;
            if(DEBUG)
            {
                LITDebug component = base.gameObject.AddComponent<LITDebug>();
            }
            Initialize();
            new LITContent().Initialize();
        }

        private void Initialize()
        {
            Assets.Initialize();
            LITConfig.Initialize(config);
            Buffs.Buffs.Initialize();
            Pickups.Initialize();


            ItemDisplays.Initialize();
            RoR2.ContentManagement.ContentManager.onContentPacksAssigned += (_) =>
            {
                ItemDisplays.FinishIDRS();
            };
        }
    }
}