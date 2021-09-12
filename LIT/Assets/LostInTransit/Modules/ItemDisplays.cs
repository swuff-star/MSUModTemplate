using UnityEngine;
using LostInTransit.Utils;
using Moonstorm;
using System.Reflection;
using RoR2.ContentManagement;

namespace LostInTransit.Modules
{
    public class ItemDisplays : ItemDisplayModuleBase
    {
        public static ItemDisplays Instance { get; set; }
        public override AssetBundle AssetBundle { get; set; } = Assets.LITAssets;
        public override SerializableContentPack ContentPack { get; set; } = LITContent.serializableContentPack;

        public override void Init()
        {
            Instance = this;
            base.Init();
            LITLogger.LogI($"Initializing Item Displays");
            PopulateVanillaIDRSFromAssetBundle(log);
            PopulateDisplayPrefabsFromAssetBundle(log);
            PopulateItemKeyAssetsFromContentPack(log);
            PopulateEquipKeyAssetsFromContentPack(log);
            PopulateMSIDRSFromAssetBundle();
            PopulateSingleItemDisplayRuleFromAssetBundle();
        }
    }
}