using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LostInTransit.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EffectDef Holder", menuName = "LostInTransit/EffectDef Holder", order = 0)]
    public class EffectDefHolder : ScriptableObject
    {
        public GameObject[] effectPrefabs;

        public static EffectDef ToEffectDef(GameObject effect)
        {
            return new EffectDef(effect);
        }
    }
}
