using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using AspectAbilities;
using System.Runtime.CompilerServices;

namespace LostInTransit.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LITAspectAbility", menuName = "LostInTransit/LITAspectAbility", order = 0)]
    public class LITAspectAbility : ScriptableObject
    {
        public EquipmentDef equipmentDef;
        public float aiMaxUseDistance;
        public AnimationCurve aiHealthFractionToUseChance;

        public AspectAbility CreateAbility()
        {
            var toReturn = new AspectAbility();
            toReturn.equipmentDef = equipmentDef;
            toReturn.aiHealthFractionToUseChance = aiHealthFractionToUseChance;
            toReturn.aiMaxUseDistance = aiMaxUseDistance;
            return toReturn;
        }
    }
}
