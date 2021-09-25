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
            IL.RoR2.Util.GetBestBodyName += GetBlightName;
            //On.RoR2.Util.GetBestBodyName += Util_GetBestBodyName;
        }

        /*private static string Util_GetBestBodyName(On.RoR2.Util.orig_GetBestBodyName orig, GameObject bodyObject)
        {
            var returnval = orig(bodyObject);
            var charBody = bodyObject.GetComponent<CharacterBody>();
            var buffDef = Assets.LITAssets.LoadAsset<BuffDef>("AffixBlighted");
            if (charBody.HasBuff(buffDef))
            {
                Debug.Log(returnval);
                foreach (BuffIndex buffIndex in BuffCatalog.eliteBuffIndices)
                {
                    if (charBody.HasBuff(buffIndex))
                    {
                        var modifierToken = Language.GetString(BuffCatalog.GetBuffDef(buffIndex).eliteDef.modifierToken);
                        modifierToken.Replace("{0}", string.Empty);
                        returnval.Replace(modifierToken, string.Empty);
                        Debug.Log(returnval);
                    }
                }
                returnval = Language.GetStringFormatted(buffDef.eliteDef.modifierToken, new object[]
                {
                    returnval
                });
                Debug.Log(returnval);
                return returnval;
            }
            else
            {
                return returnval;
            }

        }*/

        private static void GetBlightName(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.GotoNext(MoveType.After,
                x => x.MatchLdloc(0),
                x => x.MatchCallvirt<CharacterBody>("get_isElite"));
            Debug.Log(c.ToString());
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
            if (Run.instance && NetworkServer.active && !spawnedDirector)
            {
                NetworkServer.Spawn(UnityEngine.Object.Instantiate(Assets.LITAssets.LoadAsset<GameObject>("BlightedDirector")));
                spawnedDirector = true;
            }
        }
    }
}
