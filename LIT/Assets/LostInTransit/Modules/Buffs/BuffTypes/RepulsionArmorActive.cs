using RoR2;
using Moonstorm;
using LostInTransit.Items;

namespace LostInTransit.Buffs
{
    public class RepulsionArmorActive : BuffBase
    {
        public override BuffDef BuffDef { get; set; } = Assets.LITAssets.LoadAsset<BuffDef>("RepulsionArmorActive");
        public static BuffDef buff;
        public static DotController.DotIndex index;

        public override void Initialize()
        {
            buff = BuffDef;
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<RepulsionArmorActiveBehavior>(stack);
        }

        public class RepulsionArmorActiveBehavior : CharacterBody.ItemBehavior, IOnIncomingDamageServerReceiver
        {
            public void OnIncomingDamageServer(DamageInfo damageInfo)
            {
                damageInfo.damage *= ((100f - RepulsionArmor.damageResist) * 0.01f);
            }
        }
    }
}