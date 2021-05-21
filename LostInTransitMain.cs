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

namespace LostInTransit
{
    [BepInDependency(R2API.R2API.PluginGUID, R2API.R2API.PluginVersion)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin("com.swuff.LostInTransit", "Lost in Transit", "0.1.0")]
    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(LanguageAPI), nameof(PrefabAPI), nameof(ProjectileAPI), nameof(SoundAPI), nameof(LoadoutAPI), nameof(EffectAPI), nameof(ResourcesAPI), nameof(DotAPI))]

    public class LostInTransitMain : BaseUnityPlugin
    {
        public const string ModGuid = "com.swuff.LostInTransit";
        public const string ModName = "LostInTransit";
        public const string ModVer = "0.1.0";

        public const string developerPrefix = "SWF";

        public List<ItemBase> Items = new List<ItemBase>();
        public List<EquipmentBase> Equipments = new List<EquipmentBase>();
        public static Dictionary<ItemBase, bool> ItemStatusDictionary = new Dictionary<ItemBase, bool>();
        public static Dictionary<EquipmentBase, bool> EquipmentStatusDictionary = new Dictionary<EquipmentBase, bool>();
        protected readonly List<LanguageAPI.LanguageOverlay> languageOverlays = new List<LanguageAPI.LanguageOverlay>();


        public static AssetBundle MainAssets;

        public static Dictionary<string, string> ShaderLookup = new Dictionary<string, string>()
        {
            {"stubbed hopoo games/deferred/standard", "shaders/deffered/standard" },
            {"stubbed hopoo games/fx/solid parallax", "shaders/fx/solid parallax" },
            {"stubbed hopoo games/environment/distant water", "shaders/environment/distant water" },
            {"stubbed hopoo games/fx/cloud intersection remap", "shaders/fx/hgintersectioncloudremap" },
            {"stubbed hopoo games/fx/cloud remap", "shaders/fx/cloud remap" }
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

        //Tiler2 my beloved!
        //I don't need this but am leaving it just in case.
        /* public class StatHookEventArgs : EventArgs
        {
            /// <summary>Added to the direct multiplier to base health. MAX_HEALTH ~ (BASE_HEALTH + baseHealthAdd) * (HEALTH_MULT + healthMultAdd).</summary>
            public float healthMultAdd = 0f;
            /// <summary>Added to base health. MAX_HEALTH ~ (BASE_HEALTH + baseHealthAdd) * (HEALTH_MULT + healthMultAdd).</summary>
            public float baseHealthAdd = 0f;
            /// <summary>Added to base shield. MAX_SHIELD ~ BASE_SHIELD + baseShieldAdd.</summary>
            public float baseShieldAdd = 0f;
            /// <summary>Added to the direct multiplier to base health regen. HEALTH_REGEN ~ (BASE_REGEN + baseRegenAdd) * (REGEN_MULT + regenMultAdd).</summary>
            public float regenMultAdd = 0f;
            /// <summary>Added to base health regen. HEALTH_REGEN ~ (BASE_REGEN + baseRegenAdd) * (REGEN_MULT + regenMultAdd).</summary>
            public float baseRegenAdd = 0f;
            /// <summary>Added to base move speed. MOVE_SPEED ~ (BASE_MOVE_SPEED + baseMoveSpeedAdd) * (MOVE_SPEED_MULT + moveSpeedMultAdd)</summary>
            public float baseMoveSpeedAdd = 0f;
            /// <summary>Added to the direct multiplier to move speed. MOVE_SPEED ~ (BASE_MOVE_SPEED + baseMoveSpeedAdd) * (MOVE_SPEED_MULT + moveSpeedMultAdd)</summary>
            public float moveSpeedMultAdd = 0f;
            /// <summary>Added to the direct multiplier to jump power. JUMP_POWER ~ BASE_JUMP_POWER * (JUMP_POWER_MULT + jumpPowerMultAdd)</summary>
            public float jumpPowerMultAdd = 0f;
            /// <summary>Added to the direct multiplier to base damage. DAMAGE ~ (BASE_DAMAGE + baseDamageAdd) * (DAMAGE_MULT + damageMultAdd).</summary>
            public float damageMultAdd = 0f;
            /// <summary>Added to base damage. DAMAGE ~ (BASE_DAMAGE + baseDamageAdd) * (DAMAGE_MULT + damageMultAdd).</summary>
            public float baseDamageAdd = 0f;
            /// <summary>Added to attack speed. ATTACK_SPEED ~ (BASE_ATTACK_SPEED + baseAttackSpeedAdd) * (ATTACK_SPEED_MULT + attackSpeedMultAdd).</summary>
            public float baseAttackSpeedAdd = 0f;
            /// <summary>Added to the direct multiplier to attack speed. ATTACK_SPEED ~ (BASE_ATTACK_SPEED + baseAttackSpeedAdd) * (ATTACK_SPEED_MULT + attackSpeedMultAdd).</summary>
            public float attackSpeedMultAdd = 0f;
            /// <summary>Added to crit chance. CRIT_CHANCE ~ BASE_CRIT_CHANCE + critAdd.</summary>
            public float critAdd = 0f;
            /// <summary>Added to armor. ARMOR ~ BASE_ARMOR + armorAdd.</summary>
            public float armorAdd = 0f;
        }*/


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

            //Material shader autoconversion - thanks Komrade, you're da best! ^^
            var materialAssets = MainAssets.LoadAllAssets<Material>();

            foreach(Material material in materialAssets)
            {
                if (!material.shader.name.StartsWith("Stubbed")) { continue; }
                //Logger.LogInfo(material);

                var replacementShader = Resources.Load<Shader>(ShaderLookup[material.shader.name.ToLower()]);
                if (replacementShader) { material.shader = replacementShader; }

            }

            var EquipmentTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(EquipmentBase)));
            foreach (var equipmentType in EquipmentTypes)
            {
                EquipmentBase equipment = (EquipmentBase)System.Activator.CreateInstance(equipmentType);
                if (ValidateEquipment(equipment, Equipments))
                {
                    equipment.Init(Config);
                }
            }

            




        }
    }

}
