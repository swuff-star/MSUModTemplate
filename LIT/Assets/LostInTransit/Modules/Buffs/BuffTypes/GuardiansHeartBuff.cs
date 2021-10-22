using LostInTransit.Modules;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using LostInTransit.Items;
using Moonstorm;
using System.Linq;
using System;

namespace LostInTransit.Buffs
{
    public class GuardiansHeartBuff : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("GuardiansHeartBuff");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<ShackledDebuffBehavior>(stack);
        }

        public class ShackledDebuffBehavior : CharacterBody.ItemBehavior, IStatItemBehavior
        {
            public void RecalculateStatsEnd()
            {
                body.armor += GuardiansHeart.heartArmor;
            }

            public void RecalculateStatsStart()
            {
            }
        }
    }
}