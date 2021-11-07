using BepInEx;
using BepInEx.Configuration;
using LostInTransit.Modules;
using Moonstorm;
using R2API;
using R2API.Utils;
using System.Linq;
using System.Security;
using System.Security.Permissions;

#pragma warning disable CS0618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618
[module: UnverifiableCode]

namespace LostInTransit
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.TeamMoonstorm.MoonstormSharedUtils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.TheMysticSword.AspectAbilities", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        nameof(DotAPI),
        nameof(DamageAPI),
        nameof(PrefabAPI)
    })]
    public class LITMain : BaseUnityPlugin
    {
        internal const string GUID = "com.swuff.LostInTransit";
        internal const string MODNAME = "Lost in Transit";
        internal const string VERSION = "0.3.6";

        public static LITMain instance;

        public static PluginInfo pluginInfo;

        public static ConfigFile config;

        public static bool DEBUG = false;

        public void Awake()
        {
            ConfigurableFieldManager.AddMod(Config);
            TokenModifierManager.AddMod();

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
            new Modules.Projectiles().Init();
            new Pickups().Init();
            new Modules.Elites().Init();

            new ItemDisplays().Init();

            GetType().Assembly.GetTypes()
                .Where(type => typeof(EntityStates.EntityState).IsAssignableFrom(type))
                .ToList()
                .ForEach(state => HG.ArrayUtils.ArrayAppend(ref LITContent.serializableContentPack.entityStateTypes, new EntityStates.SerializableEntityStateType(state)));
        }
    }
}