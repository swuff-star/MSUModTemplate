using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moonstorm;
using RoR2;
using UnityEngine.Networking;
using LostInTransit.Buffs;

namespace LostInTransit.Components
{
    public class BlightedController : NetworkBehaviour
    {
        [SyncVar]
        public int FirstEliteIndex;

        [SyncVar]
        public int SecondEliteIndex;

        public EliteDef[] availableElites;

        public AffixBlighted.AffixBlightedBehavior buffBehavior;

        private Xoroshiro128Plus RNG;

        public void Start()
        {
            availableElites = CombatDirector.eliteTiers[1].eliteTypes;

            //Only Server handles RNG.
            if(NetworkServer.active)
                RNG = new Xoroshiro128Plus(Run.instance.runRNG.nextUlong);
            RandomizeElites();
        }

        private void RandomizeElites()
        {
            var elites = availableElites;
            if(NetworkServer.active)
            {
                FirstEliteIndex = (int)elites[RNG.RangeInt(0, elites.Length)].eliteIndex;
                //Removes the first elite index from available elites, avoids duplicates.
                elites = availableElites.Where(elite => elite.eliteIndex != (EliteIndex)FirstEliteIndex).ToArray();
                SecondEliteIndex = (int)elites[RNG.RangeInt(0, elites.Length)].eliteIndex;
            }
//            Invoke("RpcOnEliteIndexAssigned", 0.1f);
            RpcOnEliteIndexAssigned();
        }

        public void Ability()
        {
            RandomizeElites();
        }

        [ClientRpc]
        private void RpcOnEliteIndexAssigned(/*int index1, int index2*/)
        {
            buffBehavior.SetElites(FirstEliteIndex, SecondEliteIndex);
        }
        /*[Server]
        private void HostOnEliteIndexAssigned(int index1, int index2)
        {
            buffBehavior.SetElites(index1, index2);
        }*/

    }
}
