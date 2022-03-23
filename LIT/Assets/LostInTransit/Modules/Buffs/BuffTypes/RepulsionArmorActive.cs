using LostInTransit.Items;
using Moonstorm;
using RoR2;
using Moonstorm.Components;
using R2API;

namespace LostInTransit.Buffs
{
    [DisabledContent]
    public class RepulsionArmorActive : BuffBase
    {
        public override BuffDef BuffDef { get; } = LITAssets.Instance.MainAssetBundle.LoadAsset<BuffDef>("RepulsionArmorActive");

        public class RepulsionArmorActiveBehavior : BaseBuffBodyBehavior, IBodyStatArgModifier
        {
            [BuffDefAssociation(useOnClient = false, useOnServer = true)]
            public static BuffDef GetBuffDef() => LITContent.Buffs.RepulsionArmorActive;

            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.armorAdd += 500f;
            }

            /*public void OnIncomingDamageServer(DamageInfo damageInfo)
            {
                damageInfo.damage *= ((100f - RepulsionArmor.damageResist) * 0.01f);
            }*/
        }
    }
}