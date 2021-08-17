using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostInTransit.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LITEliteDef", menuName = "LostInTransit/LITEliteDef", order = 0)]
    public class LITEliteDef : EliteDef
    {
        public EliteTiers eliteTier;
        public Color lightColor = Color.clear;
        public Texture eliteRamp;
        public Material overlay;
        public Material particleMaterial;
        public GameObject effect;
    }

    public enum EliteTiers
    {
        Basic,
        PostLoop,
        Other
    }
}
