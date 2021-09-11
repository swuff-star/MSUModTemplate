/*using LostInTransit.Modules;
using RoR2;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LostInTransit.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LITSingleItemDisplayRule", menuName = "LostInTransit/LIT Single Item Display Rule")]
    public class LITSingleItemDisplayRule : ScriptableObject
    {
        #region Single Key Asset Rule Group
        [Serializable]
        public struct SingleKeyAssetRuleGroup
        {
            public string VanillaIDRSKey;

            public List<SingleItemDisplayRule> ItemDisplayRules;

            internal ItemDisplayRuleSet vanillaIDRS;
            public bool isEmpty
            {
                get
                {
                    if (ItemDisplayRules != null)
                    {
                        return ItemDisplayRules.Count == 0;
                    }
                    return true;
                }
            }

            public void AddDisplayRule(SingleItemDisplayRule itemDisplayRule)
            {
                if (ItemDisplayRules == null)
                {
                    ItemDisplayRules = new List<SingleItemDisplayRule>();
                }
                ItemDisplayRules.Add(itemDisplayRule);
            }
            public void FetchIDRS()
            {
                if(ItemDisplays.vanillaIDRS.TryGetValue(VanillaIDRSKey.ToLower(), out var value))
                {
                    if(value != null)
                    {
                        vanillaIDRS = value;
                    }
                }
            }
        }
        #endregion

        #region Single Item Display Rule
        [Serializable]
        public struct SingleItemDisplayRule
        {
            public ItemDisplayRuleType ruleType;

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
        #endregion
        public string KeyAssetName;

        public string displayPrefabName;

        [Space]
        public List<SingleKeyAssetRuleGroup> ItemDisplayRules = new List<SingleKeyAssetRuleGroup>();

        /*public ItemDisplayRuleSet.KeyAssetRuleGroup Parse(int index)
        {
            var KeyAssetToReturn = new ItemDisplayRuleSet.KeyAssetRuleGroup();
            if(Pickups.ItemPickups.TryGetValue(KeyAssetName.ToLower(), out KeyAssetToReturn.keyAsset))
            {
                KeyAssetToReturn.displayRuleGroup = new DisplayRuleGroup { rules = Array.Empty<RoR2.ItemDisplayRule>() };
            }
            else if(Pickups.EquipmentPickups.TryGetValue(KeyAssetName.ToLower(), out KeyAssetToReturn.keyAsset))
            {
                KeyAssetToReturn.displayRuleGroup = new DisplayRuleGroup { rules = Array.Empty<RoR2.ItemDisplayRule>() };
            }
            if(KeyAssetToReturn.keyAsset == null)
            {
                return new ItemDisplayRuleSet.KeyAssetRuleGroup { keyAsset = null };
            }
            for(int i = 0; i < ItemDisplayRules[index].ItemDisplayRules.Count; i++)
            {
                SingleItemDisplayRule rule = ItemDisplayRules[index].ItemDisplayRules[i];
                rule.Parse();
                var prefab = ItemDisplays.LoadDisplay(displayPrefabName.ToLower());
                HG.ArrayUtils.ArrayAppend(ref KeyAssetToReturn.displayRuleGroup.rules, new RoR2.ItemDisplayRule
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
            return KeyAssetToReturn;
        }
    }
}
*/