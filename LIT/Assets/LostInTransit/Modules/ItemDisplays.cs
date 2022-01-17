using Moonstorm;
using RoR2.ContentManagement;
using UnityEngine;

namespace LostInTransit.Modules
{
    public class ItemDisplays : ItemDisplayModuleBase
    {
        public static ItemDisplays Instance { get; set; }
        public override AssetBundle AssetBundle { get; set; } = LITAssets.Instance.MainAssetBundle;
        public override SerializableContentPack ContentPack { get; set; } = LITContent.Instance.SerializableContentPack;

        public override void Init()
        {
            Instance = this;
            base.Init();
            PopulateKeyAssetsAndDisplaysFromAssetbundle();
            PopulateMSIDRSFromAssetBundle();
            PopulateSingleItemDisplayRuleFromAssetBundle();
        }
    }
}