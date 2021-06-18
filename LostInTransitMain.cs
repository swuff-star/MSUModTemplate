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
using LostInTransit.Equipment;
using LostInTransit.Elites;

namespace LostInTransit
{
    [BepInDependency(R2API.R2API.PluginGUID, R2API.R2API.PluginVersion)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin("com.swuff.LostInTransit", "Lost in Transit", "0.1.0")]
    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(LanguageAPI), nameof(PrefabAPI), nameof(ProjectileAPI), nameof(SoundAPI), nameof(LoadoutAPI), nameof(EffectAPI), nameof(ResourcesAPI), nameof(DotAPI), nameof(EliteAPI))]

    public class LostInTransitMain : BaseUnityPlugin
    {
        public const string ModGuid = "com.swuff.LostInTransit";
        public const string ModName = "LostInTransit";
        public const string ModVer = "0.1.0";

        public const string developerPrefix = "swuff";

        public List<ItemBase> Items = new List<ItemBase>();
        public List<EquipmentBase> Equipments = new List<EquipmentBase>();
        public List<EliteBase> Elites = new List<EliteBase>();
        public static Dictionary<ItemBase, bool> ItemStatusDictionary = new Dictionary<ItemBase, bool>();
        public static Dictionary<EquipmentBase, bool> EquipmentStatusDictionary = new Dictionary<EquipmentBase, bool>();
        public static Dictionary<EliteBase, bool> EliteStatusDictionary = new Dictionary<EliteBase, bool>();
        protected readonly List<LanguageAPI.LanguageOverlay> languageOverlays = new List<LanguageAPI.LanguageOverlay>();


        public static AssetBundle MainAssets;

        public static Dictionary<string, string> ShaderLookup = new Dictionary<string, string>()
        {
            {"stubbed hopoo games/deferred/standard", "shaders/deferred/hgstandard" },
            {"stubbed hopoo games/fx/cloud intersection remap", "shaders/fx/hgintersectioncloudremap" },
            {"stubbed hopoo games/fx/cloud remap", "shaders/fx/hgcloud remap" }
        };



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
        public bool ValidateEquipment(EquipmentBase equipment, List<EquipmentBase> equipmentList)
        {
            var enabled = Config.Bind<bool>("Equipment: " + equipment.EquipmentName, "Enable Equipment?", true, "Should this equipment appear in runs?").Value;

            EquipmentStatusDictionary.Add(equipment, enabled);

            if (enabled)
            {
                equipmentList.Add(equipment);
                return true;
            }
            return false;
        }
        public bool ValidateElites(EliteBase elite, List<EliteBase> eliteList)
        {
            var enabled = Config.Bind<bool>("Elite: " + elite.EliteName, "Enable Elite?", true, "Should this elite appear in runs?").Value;

            EliteStatusDictionary.Add(elite, enabled);

            if (enabled)
            {
                eliteList.Add(elite);
            }
            return enabled;
        }

        public void Awake()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LostInTransit.lostintransit_assets"))
            {
                MainAssets = AssetBundle.LoadFromStream(stream);
            }

            var materialAssets = MainAssets.LoadAllAssets<Material>();

            foreach (Material material in materialAssets)
            {
                if (!material.shader.name.StartsWith("Stubbed")) { continue; }
                //Logger.LogInfo(material);

                var replacementShader = Resources.Load<Shader>(ShaderLookup[material.shader.name.ToLower()]);
                if (replacementShader) { material.shader = replacementShader; }

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

            //Material shader autoconversion - thanks Komrade, you're da best! ^^


            var EquipmentTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(EquipmentBase)));
            foreach (var equipmentType in EquipmentTypes)
            {
                EquipmentBase equipment = (EquipmentBase)System.Activator.CreateInstance(equipmentType);
                if (ValidateEquipment(equipment, Equipments))
                {
                    equipment.Init(Config);
                }
            }

            var EliteTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(EliteBase)));
            foreach (var eliteType in EliteTypes)
            {
                EliteBase elite = (EliteBase)System.Activator.CreateInstance(eliteType);
                if (ValidateElites(elite, Elites))
                {
                    elite.Init(Config);
                }

            }

        }
    }

}
