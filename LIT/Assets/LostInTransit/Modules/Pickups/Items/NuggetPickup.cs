using System;
using LostInTransit.Buffs;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;


namespace LostInTransit.Items
{
    internal class NuggetPickup : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            bool pickupActive = NetworkServer.active && this.alive && TeamComponent.GetObjectTeam(other.gameObject) == this.team.teamIndex;
            if(pickupActive)
            {
                CharacterBody component = other.GetComponent<CharacterBody>();
                bool canPickup = component;
                if(canPickup)
                {
                    this.alive = false;
                    if (component.GetBuffCount(NuggetRegen.buff) < RegenStacks)
                    {
                        component.AddTimedBuff(buff, this.BuffTimer * 2f);
                    }
                }
            }
        }
        public GameObject baseObject;
        public TeamFilter team;
        public float BuffTimer = 1f;
        public float RegenMult = 0.5f;
        public float RegenStacks = 1f;
        private bool alive = true;
        private BuffDef buff = NuggetRegen.buff;
    }
}
