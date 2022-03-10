using RoR2;
using UnityEngine;

namespace LostInTransit.Components
{
    [RequireComponent(typeof(ObjectScaleCurve))]
    public class LoopObjectScaleCurve : MonoBehaviour
    {
        public ObjectScaleCurve Component { get => gameObject.GetComponent<ObjectScaleCurve>(); }

        public void Update()
        {
            if (Component.time > Component.timeMax)
            {
                Component.Reset();
            }
        }
    }
}
