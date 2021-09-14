using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using RoR2;

namespace LostInTransit.Components
{
    [RequireComponent(typeof(ObjectScaleCurve))]
    public class LoopObjectScaleCurve : MonoBehaviour
    {
        public ObjectScaleCurve Component { get => gameObject.GetComponent<ObjectScaleCurve>(); }

        public void Update()
        {
            if(Component.time > Component.timeMax)
            {
                Component.Reset();
            }
        }
    }
}
