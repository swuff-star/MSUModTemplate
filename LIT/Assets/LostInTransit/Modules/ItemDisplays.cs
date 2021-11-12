using Moonstorm;
using RoR2.ContentManagement;
using UnityEngine;

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
            PopulateKeyAssetsAndDisplaysFromAssetbundle();
            PopulateMSIDRSFromAssetBundle();
            PopulateSingleItemDisplayRuleFromAssetBundle();
        }
    }
}