using LostInTransit.Items;
using Moonstorm;
using RoR2;

namespace LostInTransit.Buffs
{
    [DisabledContent]
    public class RepulsionArmorActive : BuffBase
    {
        public override BuffDef BuffDef { get; } = LITAssets.Instance.MainAssetBundle.LoadAsset<BuffDef>("RepulsionArmorActive");
        public static BuffDef buff;

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