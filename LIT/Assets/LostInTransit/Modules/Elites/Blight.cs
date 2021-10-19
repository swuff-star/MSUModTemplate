using LostInTransit.Components;
using LostInTransit.Utils;
using Moonstorm;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Elites = LostInTransit.Modules.Elites;
using static MonoMod.Cil.RuntimeILReferenceBag.FastDelegateInvokers;
using System;

namespace LostInTransit.Elites
{
    public static class Blight
    {
        public static List<EliteDef> EliteDefsForBlightedElites = new List<EliteDef>();
        private static bool spawnedDirector = false;
        internal static List<CharacterBody> blacklistedBodies = new List<CharacterBody>();
        internal static void BeginSetup()
        {
            LITLogger.LogI($"Blighted elites are enabled, setting up systems...");
            var blightedDirector = Assets.LITAssets.LoadAsset<GameObject>("BlightedDirector");
            HG.ArrayUtils.ArrayAppend(ref LITContent.serializableContentPack.networkedObjectPrefabs, blightedDirector);
            RoR2Application.onLoad += BlightSetup;
            On.RoR2.Util.GetBestBodyName += GetBlightedName;
        }

        private static string GetBlightedName(On.RoR2.Util.orig_GetBestBodyName orig, GameObject bodyObject)
        {
            var text2 = orig(bodyObject);
            CharacterBody charBody = null;
            BuffDef blightBuff = Assets.LITAssets.LoadAsset<BuffDef>("AffixBlighted");
            if ((bool)bodyObject)
            {
                charBody = bodyObject.GetComponent<CharacterBody>();
            }
            if(charBody)
            {
                if(charBody.HasBuff(blightBuff))
                {
                    var AllEliteBuffsExceptBlight = BuffCatalog.eliteBuffIndices.Where(x => x != blightBuff.buffIndex);
                    foreach (BuffIndex buffIndex in AllEliteBuffsExceptBlight)
                    {
                        if (charBody.HasBuff(buffIndex))
                        {
                            var eliteToken = Language.GetString(BuffCatalog.GetBuffDef(buffIndex).eliteDef.modifierToken);
                            eliteToken = eliteToken.Replace("{0}", string.Empty);
                            text2 = text2.Replace(eliteToken, string.Empty);
                        }
                    }
                }
                return text2;
            }
            else
            {
                return text2;
            }
        }

        private static void BlightSetup()
        {
            ModifyPrefabs();
            AddElites();
            SetupBlacklist();

            Run.onRunStartGlobal += SpawnDirector;
            LITLogger.LogI($"Finished Blighted Elite Setup.");
        }

        private static void ModifyPrefabs()
        {
            MasterCatalog.masterPrefabs
                .Where(masterPrefab => masterPrefab.GetComponent<CharacterMaster>().bodyPrefab)
                .ToList()
                .ForEach(charMaster =>
                {
                    var component = charMaster.gameObject.AddComponent<BlightedController>();
                    component.enabled = false;
                });
        }

        private static void AddElites()
        {
            List<EliteDef> availableElites = new List<EliteDef>() { RoR2Content.Elites.Fire, RoR2Content.Elites.Ice, RoR2Content.Elites.Lightning };

            for (int i = 0; i < EliteModuleBase.MoonstormElites.Count; i++)
                if (EliteModuleBase.MoonstormElites[i].eliteTier == EliteTiers.Basic)
                    availableElites.Add(EliteModuleBase.MoonstormElites[i]);

            EliteDefsForBlightedElites.AddRange(availableElites);
        }

        private static void SetupBlacklist()
        {
            string[] blacklistedBodiesNames = LITConfig.BlightBlacklist.Value.Replace(" ", string.Empty).Split(',');
            foreach(string body in blacklistedBodiesNames)
            {
                try
                {
                    var bodyComponent = BodyCatalog.GetBodyPrefabBodyComponent(BodyCatalog.FindBodyIndexCaseInsensitive(body));
                    if (bodyComponent)
                        blacklistedBodies.Add(bodyComponent);
                }
                catch (Exception e)
                {
                    LITLogger.LogE(e);
                }
            }
        }

        private static void SpawnDirector(Run obj)
        {
            if (Run.instance && NetworkServer.active)
            {
                NetworkServer.Spawn(UnityEngine.Object.Instantiate(Assets.LITAssets.LoadAsset<GameObject>("BlightedDirector")));
            }
        }
    }
}
