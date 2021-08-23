using LostInTransit.Components;
using LostInTransit.Modules;
using LostInTransit.Utils;
using RoR2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Buff = LostInTransit.Buffs.BuffBase;

namespace LostInTransit.Buffs
{
    public static class Buffs
    {
        public static BuffDef[] loadedBuffDefs
        {
            get
            {
                return LITContent.serializableContentPack.buffDefs;
            }
        }
        public static Dictionary<BuffDef, Buff> buffs = new Dictionary<BuffDef, Buff>();
        public static Dictionary<BuffDef, Material> overlayMaterials = new Dictionary<BuffDef, Material>();

        public static void Initialize()
        {
            LITLogger.LogI("Initializing Buffs...");
            typeof(Buffs).Assembly.GetTypes()
                .Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(Buff)))
                .Select(buffType => (Buff)Activator.CreateInstance(buffType))
                .ToList()
                .ForEach(buff =>
                {
                    buff.Initialize();
                    HG.ArrayUtils.ArrayAppend(ref LITContent.serializableContentPack.buffDefs, buff.BuffDef);
                    buffs.Add(buff.BuffDef, buff);
                    LITLogger.LogD($"Added Buff {buff.BuffDef.name}");
                });
            On.RoR2.CharacterBody.OnClientBuffsChanged += CheckForBuffs;
            On.RoR2.CharacterModel.UpdateOverlays += AddBuffOverlay;
        }

        private static void CheckForBuffs(On.RoR2.CharacterBody.orig_OnClientBuffsChanged orig, CharacterBody self)
        {
            orig(self);
            self.GetComponent<LITItemManager>()?.CheckForBuffs();
        }

        private static void AddBuffOverlay(On.RoR2.CharacterModel.orig_UpdateOverlays orig, CharacterModel model)
        {
            orig(model);
            if (!model.body)
                return;
            foreach (var buffKeyValue in overlayMaterials)
                if (model.body.HasBuff(buffKeyValue.Key))
                    AddOverlay(model, buffKeyValue.Value);
        }
        private static void AddOverlay(CharacterModel model, Material overlayMaterial)
        {
            if (model.activeOverlayCount >= CharacterModel.maxOverlays || !overlayMaterial)
                return;
            Material[] array = model.currentOverlays;
            int num = model.activeOverlayCount;
            model.activeOverlayCount = num + 1;
            array[num] = overlayMaterial;
        }
    }
}
