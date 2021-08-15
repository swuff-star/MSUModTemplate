using RoR2.ContentManagement;
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
        }

        private static void SwapShaders(List<Material> materials)
        {
            materials.ForEach(Material =>
            {
                if (Material.shader.name.StartsWith("StubbedShader"))
                {
                    Material.shader = Resources.Load<Shader>("shaders" + Material.shader.name.Substring(13));
                }
            });
        }
    }
}
