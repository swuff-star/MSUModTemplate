using RoR2;
using Moonstorm;

namespace LostInTransit.Buffs
{
    public class RepulsionArmorCD : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("RepulsionArmorCD");
        public static BuffDef buff;
        public static DotController.DotIndex index;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<RepulsionArmorCDBehavior>(stack);
        }

        public class RepulsionArmorCDBehavior : CharacterBody.ItemBehavior
        { } //This class is intentionally left blank.
    }
}