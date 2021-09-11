/*using LostInTransit.ScriptableObjects;
using Moonstorm;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class SOMigrator : MonoBehaviour
{
    public static void CreateAsset(Object asset, Object original)
    {
        AssetDatabase.CreateAsset(asset, AssetDatabase.GetAssetPath(original));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    #region Single item display rule migrator
    public static void CreateNewSIDR(LITSingleItemDisplayRule sidr)
    {
        var newSIDR = ScriptableObject.CreateInstance<MSSingleItemDisplayRule>();
        newSIDR.displayPrefabName = sidr.displayPrefabName;
        newSIDR.KeyAssetName = sidr.KeyAssetName;
        newSIDR.name = sidr.name + "_New";
        newSIDR.SingleItemDisplayRules = CreateSingleItemDisplayRules(sidr.ItemDisplayRules);
        CreateAsset(newSIDR, sidr);
    }
    public static List<MSSingleItemDisplayRule.SingleKeyAssetRuleGroup> CreateSingleItemDisplayRules(List<LITSingleItemDisplayRule.SingleKeyAssetRuleGroup> item)
    {
        var toReturn = new List<MSSingleItemDisplayRule.SingleKeyAssetRuleGroup>();

        foreach(var entry in item)
        {
            var keyAssetRuleGroup = new MSSingleItemDisplayRule.SingleKeyAssetRuleGroup();
            keyAssetRuleGroup.VanillaIDRSKey = entry.VanillaIDRSKey;
            keyAssetRuleGroup.ItemDisplayRules = CreateSIDRRule(entry.ItemDisplayRules);
            toReturn.Add(keyAssetRuleGroup);
        }

        return toReturn;
    }
    public static List<MSSingleItemDisplayRule.SingleItemDisplayRule> CreateSIDRRule(List<LITSingleItemDisplayRule.SingleItemDisplayRule> item)
    {
        var toReturn = new List<MSSingleItemDisplayRule.SingleItemDisplayRule>();

        foreach(var entry in item)
        {
            var singleItemDisplayRule = new MSSingleItemDisplayRule.SingleItemDisplayRule();
            singleItemDisplayRule.IDPHValues = entry.IDPHValues;
            singleItemDisplayRule.limbMask = entry.limbMask;
            singleItemDisplayRule.ruleType = entry.ruleType;
            toReturn.Add(singleItemDisplayRule);
        }
        return toReturn;
    }
    #endregion

    #region LITEliteDef
    
    public static void CreateNewEliteDef(LITEliteDef eliteDef)
    {
        var newEliteDef = ScriptableObject.CreateInstance<MSEliteDef>();
        newEliteDef.color = eliteDef.color;
        newEliteDef.effect = eliteDef.effect;
        newEliteDef.eliteEquipmentDef = eliteDef.eliteEquipmentDef;
        newEliteDef.eliteRamp = eliteDef.eliteRamp;
        newEliteDef.eliteTier = (Moonstorm.EliteTiers)eliteDef.eliteTier;
        newEliteDef.lightColor = eliteDef.lightColor;
        newEliteDef.modifierToken = eliteDef.modifierToken;
        newEliteDef.overlay = eliteDef.overlay;
        newEliteDef.particleMaterial = eliteDef.particleMaterial;
        CreateAsset(newEliteDef, eliteDef);
    }
    #endregion

    #region LITAspectAbility
    public static void CreateNewAspectAbility(LITAspectAbility aspectAbility)
    {
        var newAbility = ScriptableObject.CreateInstance<MSAspectAbility>();
        newAbility.aiHealthFractionToUseChance = aspectAbility.aiHealthFractionToUseChance;
        newAbility.aiMaxUseDistance = aspectAbility.aiMaxUseDistance;
        newAbility.equipmentDef = aspectAbility.equipmentDef;
        CreateAsset(newAbility, aspectAbility);
    }
    #endregion
}
*/