using LostInTransit.Components;
using Moonstorm;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace LostInTransit.Elites
{
    public static class Blight
    {
        public static List<EliteDef> EliteDefsForBlightedElites = new List<EliteDef>();
        private static bool spawnedDirector = false;
        internal static Dictionary<BodyIndex, int> blightCostdictionary = new Dictionary<BodyIndex, int>();
        internal static void BeginSetup()
        {
            LITLogger.LogI($"Blighted elites are enabled, setting up systems...");
            var blightedDirector = Assets.LITAssets.LoadAsset<GameObject>("BlightedDirector");
            HG.ArrayUtils.ArrayAppend(ref LITContent.serializableContentPack.networkedObjectPrefabs, blightedDirector);

            RoR2Application.onLoad += BlightSetup;
            On.RoR2.Util.GetBestBodyName += GetBlightedName;
            Stage.onServerStageBegin += CreateCosts;
        }

        private static void CreateCosts(Stage obj)
        {
            if(ClassicStageInfo.instance)
            {
                var currentStageMonsterCards = ClassicStageInfo.instance.monsterCards.Select(card => card.spawnCard)
                                                                                     .Where(card => card.prefab.GetComponent<BlightedController>());

                foreach(SpawnCard card in currentStageMonsterCards)
                {
                    var characterBody = card.prefab.GetComponent<CharacterMaster>().bodyPrefab.GetComponent<CharacterBody>();
                    if(!blightCostdictionary.ContainsKey(characterBody.bodyIndex))
                    {
                        CreateCostFromSpawnCard(card);
                    }
                }
            }
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
            if (charBody)
            {
                if (charBody.HasBuff(blightBuff))
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

            Run.onRunStartGlobal += SpawnDirector;
            LITLogger.LogI($"Finished Blighted Elite Setup.");
        }

        private static void ModifyPrefabs()
        {
            MasterCatalog.masterPrefabs
                .Where(masterPrefab => masterPrefab.GetComponent<CharacterMaster>().bodyPrefab)
                .ToList()
                .ForEach(masterPrefab =>
                {
                    var component = masterPrefab.AddComponent<BlightedController>();
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

        private static void CreateCostFromSpawnCard(SpawnCard card)
        {
            GameObject masterPrefab = card.prefab;
            CharacterMaster charMaster = masterPrefab.GetComponent<CharacterMaster>();
            if(charMaster)
            {
                GameObject bodyPrefab = charMaster.bodyPrefab;
                CharacterBody charBody = bodyPrefab?.GetComponent<CharacterBody>();
                if(charBody)
                {
                    blightCostdictionary.Add(charBody.bodyIndex, LITConfig.BindBlightCost(bodyPrefab, card));
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
