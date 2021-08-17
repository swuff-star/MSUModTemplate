using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using RoR2;
using LostInTransit.Components;

namespace LostInTransit.Utils
{
    public class LITDebug : MonoBehaviour
    {
        private void Awake()
        {
            #region networking
            //you can connect to yourself with a second instance of the game by hosting a private game with one and opening the console on the other and typing connect localhost:7777
            On.RoR2.Networking.GameNetworkManager.OnClientConnect += (self, user, t) => { };
            #endregion networking
        }
        public void Start()
        {
            #region mutemando
            //These just make testing faster
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("iHarbHD.DebugToolkit"))
            {
                Run.onRunStartGlobal += (connection) =>
                {
                    //this is because it drives me nuts when everytime i click in the inspector it fires a shot
                    var skillLocator = Resources.Load<GameObject>("prefabs/characterbodies/commandobody").GetComponent<SkillLocator>();
                    skillLocator.primary = null;
                    skillLocator.secondary = null;
                    skillLocator.utility = null;
                    skillLocator.special = null;
                    if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(DebugToolkit.DebugToolkit.GUID))
                    {
                        DebugToolkit.DebugToolkit.InvokeCMD(NetworkUser.instancesList[0], "stage1_pod", new string[] { "0" });
                        DebugToolkit.DebugToolkit.InvokeCMD(NetworkUser.instancesList[0], "no_enemies", new string[] { });
                    }
                };
            }
            #endregion
            #region IDRSHelper
            Resources.LoadAll<GameObject>("Prefabs/CharacterBodies/")
                .ToList()
                .ForEach(gameObject =>
                {
                    var modelLocator = gameObject.GetComponent<ModelLocator>();
                    if ((bool)modelLocator)
                    {
                        var mdlPrefab = modelLocator.modelTransform.gameObject;
                        if ((bool)mdlPrefab)
                        {
                            if(!mdlPrefab.GetComponent<LITIdrsHelper>())
                                mdlPrefab.AddComponent<LITIdrsHelper>();
                        }
                    }
                });
            #endregion
        }
    }
}
