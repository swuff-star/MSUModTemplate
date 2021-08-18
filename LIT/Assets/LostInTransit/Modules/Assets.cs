using LostInTransit.ScriptableObjects;
using RoR2.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Path = System.IO.Path;

namespace LostInTransit
{
    internal static class Assets
    {
        internal static string assemblyDir
        {
            get
            {
                return Path.GetDirectoryName(LITMain.pluginInfo.Location);
            }
        }
        private const string mainAssetBundle = "litassets";

        public static AssetBundle LITAssets { get; } = AssetBundle.LoadFromFile(Path.Combine(assemblyDir, mainAssetBundle));

        internal static void Initialize()
        {
            LITContent.serializableContentPack = LITAssets.LoadAsset<SerializableContentPack>("ContentPack");
            SwapShaders(LITAssets.LoadAllAssets<Material>().ToList());
            LoadEffects(LITAssets.LoadAllAssets<EffectDefHolder>().ToList());
        }

        private static void LoadEffects(List<EffectDefHolder> effectDefHolders)
        {
            effectDefHolders.ForEach(effectDef =>
            {
                effectDef.effectPrefabs
                .ToList()
                .ForEach(effectPrefab =>
                {
                    HG.ArrayUtils.ArrayAppend(ref LITContent.serializableContentPack.effectDefs, EffectDefHolder.ToEffectDef(effectPrefab));
                });
            });
        }

        private static void SwapShaders(List<Material> materials)
        {
            materials.ForEach(Material =>
            {
                Debug.Log("Swapping material.");
                if (Material.shader.name.StartsWith("StubbedShader"))
                {
                    Material.shader = Resources.Load<Shader>("shaders" + Material.shader.name.Substring(13));
                }
            });
        }
    }
}
