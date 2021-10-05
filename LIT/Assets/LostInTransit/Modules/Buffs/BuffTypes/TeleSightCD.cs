using RoR2;
using Moonstorm;

namespace LostInTransit.Buffs
{
    public class TeleSightCD : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("TeleSightCD");
        public static BuffDef buff;
        public static DotController.DotIndex index;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<TeleSightCDBehavior>(stack);
        }

        public class TeleSightCDBehavior : CharacterBody.ItemBehavior
        { } //This class is intentionally left blank.
    }
}