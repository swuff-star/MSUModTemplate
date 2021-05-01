using BepInEx;
using R2API;
using R2API.Utils;
using System.Reflection;
using UnityEngine;
using System;
using RoR2;
using System.Linq;
using LostInTransit.Items;
using System.Collections.Generic;

namespace LostInTransit
{
    [BepInDependency(R2API.R2API.PluginGUID, R2API.R2API.PluginVersion)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin("com.swuff.LostInTransit", "Lost in Transit", "0.1.0")]
    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(LanguageAPI), nameof(PrefabAPI))]

    public class LostInTransitMain : BaseUnityPlugin
    {
        public const string ModGuid = "com.swuff.LostInTransit";
        public const string ModName = "Lost In Transit";
        public const string ModVer = "0.1.0";



        public List<ItemBase> Items = new List<ItemBase>();
        public static Dictionary<ItemBase, bool> ItemStatusDictionary = new Dictionary<ItemBase, bool>();
        protected readonly List<LanguageAPI.LanguageOverlay> languageOverlays = new List<LanguageAPI.LanguageOverlay>();


        public static AssetBundle MainAssets;

        public bool ValidateItem(ItemBase item, List<ItemBase> itemList)
        {
            var enabled = Config.Bind<bool>("Item: " + item.ItemName, "Enable Item?", true, "Should this item appear in runs?").Value;
            var aiBlacklist = Config.Bind<bool>("Item: " + item.ItemName, "Blacklist Item from AI Use?", false, "Should the AI not be able to obtain this item?").Value;

            ItemStatusDictionary.Add(item, enabled);

            if (enabled)
            {
                itemList.Add(item);
               /* if (aiBlacklist)
                {
                    item.AIBlacklisted = true;
                } */
            }
            return enabled;
        }

        public void Awake()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LostInTransit.lostintransit_assets"))
            {
                MainAssets = AssetBundle.LoadFromStream(stream);
            }

            var ItemTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(ItemBase)));
            foreach (var itemType in ItemTypes)
            {
                ItemBase item = (ItemBase)System.Activator.CreateInstance(itemType);
                if (ValidateItem(item, Items))
                {
                    item.Init(Config);
                }
            }



        }
    }

}
