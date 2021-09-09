using LostInTransit.Modules;
using R2API;
using RoR2;
using System;

namespace LostInTransit.Buffs
{
    public class GoldenGunBuff : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("GoldenGun");
        public static BuffDef buff;
        public static DotController.DotIndex index;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<GoldenGunBuffBehavior>(stack);
        }

        public class GoldenGunBuffBehavior : CharacterBody.ItemBehavior, IStatItemBehavior
        {
            public void RecalcStatsEnd()
            {
                body.damage += (stack * 0.01f) * (body.baseDamage + (body.levelDamage * (body.level - 1)));
            }

            public void RecalcStatsStart()
            {
            }
        }
    }
}