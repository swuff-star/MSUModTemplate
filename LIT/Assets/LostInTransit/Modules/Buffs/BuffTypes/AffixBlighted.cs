using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LostInTransit.Modules;
using UnityEngine.Networking;
using LostInTransit.Utils;
using LostInTransit.Components;
using Random = System.Random;

namespace LostInTransit.Buffs
{
    public class AffixBlighted : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("BuffAffixBlighted");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<AffixBlightedBehavior>(stack);
        }

        public class AffixBlightedBehavior : CharacterBody.ItemBehavior
        {

            public void Awake()
            {
                //var component = AbilityEffect.AddComponent<DestroyOnTimer>();
                //Debug.Log(EliteCatalog.eliteList);
                var random = new Random();
                var list = EliteCatalog.eliteList;
                int index = random.Next(list.Count);
                Debug.Log(list[index]);
                //★ idk what the fuck i'm doing here.
                //★ this is a disaster and it sucks - figure it out later!

            }
            public void Update()
            {
                //Debug.Log(EliteCatalog.eliteList);
            
            }


        }
    }
}
