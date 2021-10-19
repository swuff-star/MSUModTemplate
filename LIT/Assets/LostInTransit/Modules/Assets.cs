using Moonstorm;
using LostInTransit.Utils;
using RoR2.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Path = System.IO.Path;
using System.IO;
using R2API;

namespace LostInTransit
{
    internal static class Assets
    {
        internal static string AssemblyDir
        {
            get
            {
                return Path.GetDirectoryName(LITMain.pluginInfo.Location);
            }
        }

        private static string MainAssetBundlePath { get => Path.Combine(AssemblyDir, mainAssetBundle); }
        private static string SoundBankPath { get => Path.Combine(AssemblyDir, soundBank); }

        private const string mainAssetBundle = "litassets";
        private const string soundBank = "LostInTransitSBNK.bnk";

        public static AssetBundle LITAssets { get; } = AssetBundle.LoadFromFile(MainAssetBundlePath);

        public static Material[] cloudRemaps = Array.Empty<Material>();

        internal static void Initialize()
        {
            LITContent.serializableContentPack = LITAssets.LoadAsset<SerializableContentPack>("ContentPack");
            SwapShaders(LITAssets.LoadAllAssets<Material>().ToList());
            LoadEffects(LITAssets.LoadAllAssets<EffectDefHolder>().ToList());
            LoadSoundBank();
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
            var cloudMat = Resources.Load<GameObject>("Prefabs/Effects/OrbEffects/LightningStrikeOrbEffect").transform.Find("Ring").GetComponent<ParticleSystemRenderer>().material;
            materials.ForEach(Material =>
            {
                if (Material.shader.name.StartsWith("StubbedShader"))
                {
                    Material.shader = Resources.Load<Shader>("shaders" + Material.shader.name.Substring(13));
                    if (Material.shader.name.Contains("Cloud Remap"))
                    {
                        var eatShit = new RuntimeCloudMaterialMapper(Material);
                        Material.CopyPropertiesFromMaterial(cloudMat);
                        eatShit.SetMaterialValues(ref Material);
                        HG.ArrayUtils.ArrayAppend(ref cloudRemaps, Material);
                    }
                }
            });
        }

        private static void LoadSoundBank()
        {
            byte[] array = File.ReadAllBytes(SoundBankPath);
            SoundAPI.SoundBanks.Add(array);
        }
    }
}
