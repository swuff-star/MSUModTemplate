using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LostInTransit.Modules;
using System.Globalization;

namespace LostInTransit.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LITIDRS", menuName = "LostInTransit/LITIDRS", order = 0)]
    public class LITIDRS : ScriptableObject
    {
        public static List<LITIDRS> instancesList = new List<LITIDRS>();
        internal ItemDisplayRuleSet vanillaIDRS;

        #region KeyAssetRuleGroup
        [Serializable]
        public struct KeyAssetRuleGroup
        {
            public string keyAssetName;
            public List<ItemDisplayRule> rules;

            public bool isEmpty
            {
                get
                {
                    if(rules != null)
                    {
                        return rules.Count == 0;
                    }
                    return true;
                }
            }

            public void AddDisplayRule(ItemDisplayRule itemDisplayRule)
            {
                if (rules == null)
                {
                    rules = new List<ItemDisplayRule>();
                }
                rules.Add(itemDisplayRule);
            }
        }
        #endregion KeyAssetruleGroup

        #region ItemDisplayRule
        [Serializable]
        public struct ItemDisplayRule
        {
            public ItemDisplayRuleType ruleType;

            public string displayPrefabName;

            [Tooltip("Values taken from the ItemDisplayPlacementHelper\nMake sure to use the copy format \"For Parsing\"!.")]
            [TextArea(1, int.MaxValue)]
            public string IDPHValues;

            public LimbFlags limbMask;

            internal string childName;
            internal Vector3 localPos;
            internal Vector3 localAngles;
            internal Vector3 localScale;

            private List<string> V3Builder;

            internal const string constant = "NoValue";

            public void Parse()
            {
                if (IDPHValues == string.Empty)
                {
                    childName = constant;
                    localPos = new Vector3(0, 0, 0);
                    localAngles = new Vector3(0, 0, 0);
                    localScale = new Vector3(1, 1, 1);
                    return;
                }
                List<string> splitValues = IDPHValues.Split(',').ToList();
                childName = splitValues[0];
                V3Builder = new List<string>();
                V3Builder.Clear();
                V3Builder.Add(splitValues[1]);
                V3Builder.Add(splitValues[2]);
                V3Builder.Add(splitValues[3]);
                localPos = CreateVector3FromList(V3Builder);

                V3Builder.Clear();
                V3Builder.Add(splitValues[4]);
                V3Builder.Add(splitValues[5]);
                V3Builder.Add(splitValues[6]);
                localAngles = CreateVector3FromList(V3Builder);

                V3Builder.Clear();
                V3Builder.Add(splitValues[7]);
                V3Builder.Add(splitValues[8]);
                V3Builder.Add(splitValues[9]);
                localScale = CreateVector3FromList(V3Builder);
            }
            private Vector3 CreateVector3FromList(List<string> list)
            {
                Vector3 toReturn = new Vector3(float.Parse(list[0], CultureInfo.InvariantCulture), float.Parse(list[1], CultureInfo.InvariantCulture), float.Parse(list[2], CultureInfo.InvariantCulture));

                return toReturn;
            }
        }
        #endregion ItemDisplayRule

        [Space]
        public List<KeyAssetRuleGroup> LITKeyAssetRuleGroups = new List<KeyAssetRuleGroup>();
        public string VanillaIDRSKey;

        public void Awake()
        {
            instancesList.Add(this);
        }
        public void FetchIDRS()
        {
            if (ItemDisplays.vanillaIDRS.TryGetValue(VanillaIDRSKey.ToLower(), out var value))
            {
                if (value != null)
                {
                    vanillaIDRS = value;
                }
            }
        }

        public ItemDisplayRuleSet.KeyAssetRuleGroup[] GetItemDisplayRules()
        {
            var keyAssetList = new List<ItemDisplayRuleSet.KeyAssetRuleGroup>();
            foreach (var SS2KeyAssetRuleGroup in LITKeyAssetRuleGroups)
            {
                var keyAssetGroup = new ItemDisplayRuleSet.KeyAssetRuleGroup();
                if (Pickups.ItemPickups.TryGetValue(SS2KeyAssetRuleGroup.keyAssetName.ToLower(), out keyAssetGroup.keyAsset))
                {
                    keyAssetGroup.displayRuleGroup = new DisplayRuleGroup { rules = Array.Empty<RoR2.ItemDisplayRule>() };
                }
                else if (Pickups.EquipmentPickups.TryGetValue(SS2KeyAssetRuleGroup.keyAssetName.ToLower(), out keyAssetGroup.keyAsset))
                {
                    keyAssetGroup.displayRuleGroup = new DisplayRuleGroup { rules = Array.Empty<RoR2.ItemDisplayRule>() };
                }
                if (keyAssetGroup.keyAsset == null)
                {
                    continue;
                }
                for (int i = 0; i < SS2KeyAssetRuleGroup.rules.Count; i++)
                {
                    ItemDisplayRule rule = SS2KeyAssetRuleGroup.rules[i];
                    rule.Parse();
                    var prefab = ItemDisplays.LoadDisplay(rule.displayPrefabName.ToLower());
                    HG.ArrayUtils.ArrayAppend(ref keyAssetGroup.displayRuleGroup.rules, new RoR2.ItemDisplayRule
                    {
                        ruleType = rule.ruleType,
                        followerPrefab = prefab,
                        childName = rule.childName,
                        localPos = rule.localPos,
                        localAngles = rule.localAngles,
                        localScale = rule.localScale,
                        limbMask = rule.limbMask
                    });
                }
                keyAssetList.Add(keyAssetGroup);
            }
            return keyAssetList.ToArray();
        }
    }
}
