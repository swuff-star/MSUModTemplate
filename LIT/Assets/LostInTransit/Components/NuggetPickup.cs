using System;
using LostInTransit.Buffs;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;


namespace LostInTransit.Components
{
    internal class NuggetPickup : MonoBehaviour
    {
        public GameObject baseObject;
        public TeamFilter team;

        [HideInInspector]
        public float RegenStacks, BuffTimer, RegenMult;

        [SerializeField]
        private BuffDef buff;

        private bool alive = true;

        private void OnTriggerStay(Collider other)
        {
            if (!NetworkServer.active || !alive)
                return;
            TeamIndex objTeam = TeamComponent.GetObjectTeam(other.gameObject);
            if(objTeam == team.teamIndex)
            {
                CharacterBody body = other.GetComponent<CharacterBody>();
                if(body)
                {
                    alive = false;
                    if(body.GetBuffCount(buff) < RegenStacks)
                    {
                        body.AddTimedBuff(buff, BuffTimer * 2);
                    }
                    else
                    {
                        body.timedBuffs.Find(tb => tb.buffIndex == buff.buffIndex).timer = BuffTimer * 2;
                    }
                    Destroy(baseObject);
                }
            }
        }
    }
}
