using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LostInTransit.Utils;
using LostInTransit.ScriptableObjects;
using LostInTransit.Components;
using Object = UnityEngine.Object;

namespace LostInTransit.Modules
{
    internal static class ItemDisplays
    {
        public static Dictionary<string, GameObject> itemDisplayPrefabs = new Dictionary<string, GameObject>();
        public static Dictionary<string, ItemDisplayRuleSet> vanillaIDRS = new Dictionary<string, ItemDisplayRuleSet>();

        internal static void Initialize()
        {
            LITLogger.LogI("Initializing Item Displays...");
            LITLogger.LogD("RoR2 Item Display Prefab Names");
            PopulateFromBody("Commando");
            PopulateFromBody("LunarExploder");
            LITLogger.LogD("-------------------");
            LITLogger.LogD("IDRS names");
            PopulateVanillaIDRS();
            LITLogger.LogD("-------------------");
            LITLogger.LogD("LIT Item Display Prefab Names names");
            Assets.LITAssets.LoadAllAssets<LITItemDisplays>()
                .ToList()
                .ForEach(itemDisplay =>
                {
                    itemDisplay.itemDisplays
                    .ToList()
                    .ForEach(display =>
                    {
                        LITLogger.LogD(display.name);
                        itemDisplayPrefabs.Add(display.name.ToLower(), display);
                    });
                });
            LITLogger.LogD("-------------------");
            LITLogger.LogD("RoR2 Item names");
            typeof(RoR2Content.Items)
                .GetFields()
                .ToList()
                .ForEach(Field =>
                {
                    LITLogger.LogD(Field.Name);
                    Pickups.ItemPickups.Add(Field.Name.ToLower(), Field.GetValue(typeof(ItemDef)) as Object);
                });
            LITLogger.LogD("-------------------");
            LITLogger.LogD("RoR2 Equip Names");
            typeof(RoR2Content.Equipment)
                .GetFields()
                .ToList()
                .ForEach(Field =>
                {
                    LITLogger.LogD(Field.Name);
                    Pickups.EquipmentPickups.Add(Field.Name.ToLower(), Field.GetValue(typeof(EquipmentDef)) as Object);
                });
            LITLogger.LogD("-------------------");
            LITLogger.LogD("LIT ItemDef Names");
            LITContent.serializableContentPack.itemDefs
                .ToList()
                .ForEach(itemDef =>
                {
                    LITLogger.LogD(itemDef.name);
                    Pickups.ItemPickups.Add(itemDef.name.ToLower(), itemDef);
                });
            LITLogger.LogD("-------------------");
            LITLogger.LogD("LIT EquipmentDef Names");
            LITContent.serializableContentPack.equipmentDefs
                .ToList()
                .ForEach(equipmentDef =>
                {
                    LITLogger.LogD(equipmentDef.name);
                    Pickups.EquipmentPickups.Add(equipmentDef.name.ToLower(), equipmentDef);
                });
        }

        internal static void FinishIDRS()
        {
            LITLogger.LogI("Finishing IDRS");
            Assets.LITAssets.LoadAllAssets<LITIDRS>()
                .ToList()
                .ForEach(IDRS =>
                {
                    IDRS.FetchIDRS();
                    if (IDRS.vanillaIDRS)
                    {
                        IDRS.GetItemDisplayRules()
                        .ToList()
                        .ForEach(itemDisplayRule =>
                        {
                            HG.ArrayUtils.ArrayAppend(ref IDRS.vanillaIDRS.keyAssetRuleGroups, itemDisplayRule);
                        });
                    }
                });
            var SingleItemDisplayRules = Assets.LITAssets.LoadAllAssets<LITSingleItemDisplayRule>();
            for (int i = 0; i < SingleItemDisplayRules.Length; i++)
            {
                var currentI = SingleItemDisplayRules[i];
                for(int j = 0; j < currentI.ItemDisplayRules.Count; j++)
                {
                    var currentJ = currentI.ItemDisplayRules[j];
                    currentJ.FetchIDRS();
                    for (int k = 0; k < currentJ.ItemDisplayRules.Count; k++)
                    {
                        var currentK = currentJ.ItemDisplayRules[k];
                        var toAppend = currentI.Parse(j);
                        if (toAppend.keyAsset != null)
                            HG.ArrayUtils.ArrayAppend(ref currentJ.vanillaIDRS.keyAssetRuleGroups, toAppend);
                    }
                }
            }
        }

        private static void PopulateFromBody(string bodyName)
        {
            ItemDisplayRuleSet itemDisplayRuleSet = Resources.Load<GameObject>("Prefabs/CharacterBodies/" + bodyName + "Body").GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet;
            
            ItemDisplayRuleSet.KeyAssetRuleGroup[] item = itemDisplayRuleSet.keyAssetRuleGroups;

            for(int i = 0; i < item.Length; i++)
            {
                ItemDisplayRule[] rules = item[i].displayRuleGroup.rules;
                for(int j = 0; j < rules.Length; j++)
                {
                    GameObject followerPrefab = rules[j].followerPrefab;
                    if(followerPrefab)
                    {
                        string name = followerPrefab.name;
                        string key = name?.ToLower();
                        LITLogger.LogD(followerPrefab.name);
                        if(!itemDisplayPrefabs.ContainsKey(key))
                        {
                            itemDisplayPrefabs[key] = followerPrefab;
                        }
                    }
                }
            }
        }
    
        private static void PopulateVanillaIDRS()
        {
            List<ItemDisplayRuleSet> IDRSList = new List<ItemDisplayRuleSet>();
            Resources.LoadAll<GameObject>("Prefabs/CharacterBodies/")
                .ToList()
                .ForEach(gameObject =>
                {
                    var modelLocator = gameObject.GetComponent<ModelLocator>();
                    if ((bool)modelLocator)
                    {
                        var mdlPrefab = modelLocator.modelTransform.gameObject;
                        if ((bool)mdlPrefab)
                        {
                            var characterModel = mdlPrefab.GetComponent<CharacterModel>();
                            if ((bool)characterModel)
                            {
                                var IDRS = characterModel.itemDisplayRuleSet;
                                if ((bool)IDRS)
                                {
                                    bool flag = vanillaIDRS.ContainsKey(IDRS.name.ToLower());
                                    if (!flag)
                                    {
                                        LITLogger.LogD(IDRS.name);
                                        vanillaIDRS.Add(IDRS.name.ToLower(), IDRS);
                                    }
                                }
                            }
                        }
                    }
                });
        }

        internal static GameObject LoadDisplay(string name)
        {
            if (itemDisplayPrefabs.ContainsKey(name.ToLower()))
            {
                if (itemDisplayPrefabs[name.ToLower()]) return itemDisplayPrefabs[name.ToLower()];
            }

            return null;
        }
    }
}