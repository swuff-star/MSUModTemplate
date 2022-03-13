using Moonstorm;
using RoR2;

namespace LostInTransit.Buffs
{
    public class Shackled : BuffBase
    {
        public override BuffDef BuffDef { get; } = LITAssets.Instance.MainAssetBundle.LoadAsset<BuffDef>("Shackled");
        public static BuffDef buff;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        //G - this still uses IStatItemBehaviour because the slow needs to be applied after all other modifiers have been added, which isn't supported by RecalcStatsAPI yet
        public class ShackledDebuffBehavior : CharacterBody.ItemBehavior, IStatItemBehavior
        {
            public void RecalculateStatsEnd()
            {
                body.attackSpeed *= (1 - Items.PrisonShackles.slowMultiplier);
            }

            public void RecalculateStatsStart()
            {
            }
        }
    }
}