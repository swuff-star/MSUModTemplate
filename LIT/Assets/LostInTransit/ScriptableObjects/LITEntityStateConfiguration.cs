using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LostInTransit.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New EntityStateConfiguration", menuName = "LostInTransit/EntityStateConfiguration", order = 0)]
    public class LITEntityStateConfiguration : EntityStateConfiguration
    {
        public SerializableEntityStateType StateType;

        public void Awake()
        {
            base.targetType = (HG.SerializableSystemType)StateType.stateType;
            base.Awake();
        }
    }
}
