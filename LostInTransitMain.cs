using BepInEx;
using R2API;
using R2API.Utils;
using System.Reflection;
using UnityEngine;

namespace LostInTransit
{
    [BepInDependency(R2API.R2API.PluginGUID, R2API.R2API.PluginVersion)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin("com.swuff.LostInTransit", "Lost in Transit", "0.1.0")]
    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(LanguageAPI))]

    public class LostInTransit : BaseUnityPlugin
    {
        public const string ModGuid = "com.swuff.LostInTransit";
        public const string ModName = "Lost In Transit";
        public const string ModVer = "0.1.0";

        public static AssetBundle MainAssets;
        public void Awake()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LostInTransit.lostintransit_assets"))
            {
                MainAssets = AssetBundle.LoadFromStream(stream);
            }

        }
    }
}
