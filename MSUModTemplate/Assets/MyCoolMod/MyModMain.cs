using BepInEx;
using BepInEx.Configuration;
using HG.Reflection;
using MyMod.Modules;
using Moonstorm;
using R2API;
using R2API.Utils;
using System.Security;
using System.Security.Permissions;

#pragma warning disable CS0618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618
[module: UnverifiableCode]
[assembly: SearchableAttribute.OptIn]

namespace MyMod                                             //Right Click -> "Rename" this, or I will deprecate the fuck out of your mod.
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.TeamMoonstorm.MoonstormSharedUtils", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        //nameof(PrefabAPI)
        //Any R2API submodules should be listed here, separated by colons.
        //PrefabAPI is left here as an example; you'll probably want to uncomment it.
    })]
    public class MyModMain : BaseUnityPlugin                //Right Click -> "Rename" the "MyModMain" class, as well.
    {
        internal const string GUID = "com.Me.MyCoolMod";    //Standard naming convention is "com.author.modname".
        internal const string MODNAME = "My Cool Mod";      //Your mod's name.
        internal const string VERSION = "0.0.0";            //The version of your mod. The first digit is for the "major version", usually only incremented for major releases.
                                                            //The second digit is the "minor version", generally incremented for smaller versions. 
        public static MyModMain instance;                   //The final digit is the "revision", incremented for hotfixes / patches / etc.

        public static PluginInfo pluginInfo;

        public static ConfigFile config;

        public static bool DEBUG = false;

        public void Awake()
        {
            instance = this;
            pluginInfo = Info;
            config = Config;
            MyModLogger.logger = Logger;    //Rename "MyModLogger",

            new MyModConfig().Init();       //"MyModConfig",
            new MyModAssets().Init();       //"MyModAssets",
            new MyModContent().Init();      //"MyModContent",
            new MyModLanguage().Init();     //and "MyModLanguage".

            ConfigurableFieldManager.AddMod(this);
            TokenModifierManager.AddToManager();
        }
    }
}