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
        public void Start()
        {
            Debug.Log("Debug!");
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
        }
    }
}
