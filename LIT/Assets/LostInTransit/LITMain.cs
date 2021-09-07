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
        internal const string VERSION = "0.1.3";

        public static LITMain instance;

        public static PluginInfo pluginInfo;

        public static ConfigFile config;

        public static bool DEBUG = false;

        public static bool AspectAbilitiesInstalled = false;

        public void Awake()
        {
            instance = this;

            pluginInfo = Info;

            config = Config;

            LITLogger.logger = Logger;

            AspectAbilitiesInstalled = CheckForExternalMod("com.TheMysticSword.AspectAbilities");
            Debug.Log(AspectAbilitiesInstalled);


            if (DEBUG)
            {
                LITDebug component = base.gameObject.AddComponent<LITDebug>();
            }
            Initialize();
            new LITContent().Initialize();
        }

        private void Initialize()
        {
            Assets.Initialize();
            Interfaces.Initialize();
            LITLanguage.Initialize();
            LITConfig.Initialize(config);
            Buffs.Buffs.Initialize();
            DamageTypes.DamageTypes.Initialize();
            Pickups.Initialize();
            Elites.Initialize();


            ItemDisplays.Initialize();
            RoR2.ContentManagement.ContentManager.onContentPacksAssigned += (_) =>
            {
                ItemDisplays.FinishIDRS();
            };
        }

        private bool CheckForExternalMod(string GUID)
        {
            var hasMod = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(GUID);
            if(hasMod)
            {
                LITLogger.LogI($"Plugin {GUID} detected, enabling crosscompat.");
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}