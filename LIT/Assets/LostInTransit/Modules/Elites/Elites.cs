using LostInTransit.Elites;
using Moonstorm;
using RoR2;
using RoR2.ContentManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LostInTransit.Modules
{
    public class Elites : EliteModuleBase
    {
        public static Elites Instance { get; set; }
        public static EliteDef[] LoadedLITElites { get => LITContent.serializableContentPack.eliteDefs; }
        public static MSEliteDef[] LoadedLITElitesAsMSElites { get => LITContent.serializableContentPack.eliteDefs as MSEliteDef[]; }
        public override SerializableContentPack ContentPack { get; set; } = LITContent.serializableContentPack;
        public override AssetBundle AssetBundle { get; set; } = Assets.LITAssets;

        public override void Init()
        {
            Instance = this;
            base.Init();
            LITLogger.LogI($"Initializing Elites...");
            GetNonInitializedEliteEquipments();
        }

        public override IEnumerable<EliteEquipmentBase> GetNonInitializedEliteEquipments()
        {
            base.GetNonInitializedEliteEquipments()
                .Where(elite => LITMain.config.Bind<bool>("Lost in Transit Elites", elite.EliteDef.name, true, "Enable/disable this Elite Type.").Value)
                .ToList()
                .ForEach(elite => AddElite(elite, ContentPack));
            LateEliteSetup();
            return null;
        }

        private void LateEliteSetup()
        {
            if (MoonstormElites.Contains(Assets.LITAssets.LoadAsset<MSEliteDef>("Volatile")))
            {
                VolatileSpitebomb.BeginSetup();
            }
            if (MoonstormElites.Contains(Assets.LITAssets.LoadAsset<MSEliteDef>("Blighted")))
            {
                Blight.BeginSetup();
            }
            if (MoonstormElites.Contains(Assets.LITAssets.LoadAsset<MSEliteDef>("Leeching")))
            {
                RoR2Application.onLoad += () =>
                {
                    var grandpa = Resources.Load<GameObject>("prefabs/characterbodies/grandparentbody");
                    Debug.Log(grandpa);
                    if(grandpa)
                    {
                        var charLoc = grandpa.GetComponentInChildren<ChildLocator>();
                        Debug.Log(charLoc);
                        if(charLoc)
                        {
                            var headChild = charLoc.FindChild("Head");
                            Debug.Log(headChild);
                            HG.ArrayUtils.ArrayAppend(ref charLoc.transformPairs, new ChildLocator.NameTransformPair
                            {
                                name = "RingBottom",
                                transform = headChild.Find("head.2")
                            });
                            HG.ArrayUtils.ArrayAppend(ref charLoc.transformPairs, new ChildLocator.NameTransformPair
                            {
                                name = "RingMiddle",
                                transform = headChild.Find("head.2/head.3")
                            });
                            HG.ArrayUtils.ArrayAppend(ref charLoc.transformPairs, new ChildLocator.NameTransformPair
                            {
                                name = "RingTop",
                                transform = headChild.Find("head.2/head.3/head.4")
                            });
                        }
                    }
                };
            }
        }
    }
}