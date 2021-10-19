using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostInTransit.Components
{
    [RequireComponent(typeof(Transform))]
    public class ScaleByParentTransform : MonoBehaviour
    {
        [SerializeField]
        private Transform parentTransform;

        private void Start()
        {
            if(parentTransform == null)
            {
                parentTransform = gameObject.transform.parent;
            }
        }

        private void Update()
        {
            transform.localScale = parentTransform.localScale;
        }
    }
}
