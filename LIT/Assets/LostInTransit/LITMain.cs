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
    [BepInDependency(R2API.R2API.PluginGUID, BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.TeamMoonstorm.MoonstormSharedUtils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.TheMysticSword.AspectAbilities", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        nameof(DotAPI),
        nameof(DamageAPI)
    })]
    public class LITMain : BaseUnityPlugin
    {
        internal const string GUID = "com.swuff.LostInTransit";
        internal const string MODNAME = "Lost in Transit";
        internal const string VERSION = "0.2.0";

        public static LITMain instance;

        public static PluginInfo pluginInfo;

        public static ConfigFile config;

        public static bool DEBUG = false;

        public void Awake()
        {
            instance = this;
            pluginInfo = Info;
            config = Config;
            LITLogger.logger = Logger;

            /*if (DEBUG)
            {
                LITDebug component = base.gameObject.AddComponent<LITDebug>();
            }*/

            Initialize();
            new LITContent().Initialize();
        }

        private void Initialize()
        {
            Assets.Initialize();
            LITLanguage.Initialize();
            LITConfig.Initialize(config);

            new Buffs.Buffs().Init();
            new DamageTypes.DamageTypes().Init();
            new Pickups().Init();
            new Elites().Init();

            new ItemDisplays().Init();
        }
    }
}