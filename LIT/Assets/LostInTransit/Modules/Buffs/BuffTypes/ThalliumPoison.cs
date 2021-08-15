using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostInTransit.Buffs
{
    public class ThalliumPoison : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("buffThalliumPoison");
        public static BuffDef buff;
        public static DotController.DotIndex index;

        public override void Initialize()
        {
            buff = BuffDef;
            index = DotAPI.RegisterDotDef(1, 0.5f, DamageColorIndex.Poison, BuffDef);
        }
    }
}