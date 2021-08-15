using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostInTransit.Equipments
{
    public abstract class EquipmentBase
    {
        public abstract EquipmentDef EquipmentDef { get; set; }

        public virtual void Initialize() { }

        public virtual void AddBehavior(ref CharacterBody body, int stack) { }

        public virtual bool FireAction(EquipmentSlot slot)
        {
            return false;
        }
    }
}