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
using static MonoMod.Cil.RuntimeILReferenceBag.FastDelegateInvokers;

namespace LostInTransit.Modules
{
    public static class Blight
    {
        public static List<EliteDef> EliteDefsForBlightedElites = new List<EliteDef>();
        private static bool spawnedDirector = false;
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
            MasterCatalog.masterPrefabs
                .Where(masterPrefab => masterPrefab.GetComponent<CharacterMaster>().bodyPrefab)
                .ToList()
                .ForEach(charMaster =>
                {
                    var component = charMaster.gameObject.AddComponent<BlightedController>();
                    component.enabled = false;
                });
            List<EliteDef> availableElites = new List<EliteDef>() { RoR2Content.Elites.Fire, RoR2Content.Elites.Ice, RoR2Content.Elites.Lightning };

            for (int i = 0; i < Elites.MoonstormElites.Count; i++)
                if (Elites.MoonstormElites[i].eliteTier == EliteTiers.Basic)
                    availableElites.Add(Elites.MoonstormElites[i]);

            EliteDefsForBlightedElites.AddRange(availableElites);


            Run.onRunStartGlobal += SpawnDirector;
            LITLogger.LogI($"Finished MasterPrefab modifications.");
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
