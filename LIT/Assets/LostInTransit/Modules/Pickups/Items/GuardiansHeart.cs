using RoR2;
using System;
using LostInTransit.Buffs;
using UnityEngine.Networking;
using Moonstorm;
using UnityEngine;

namespace LostInTransit.Items
{
    [DisabledContent]
    public class GuardiansHeart : ItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("GuardiansHeart");

        [ConfigurableField(ConfigName = "Shield per Heart", ConfigDesc = "Amount of shield added per heart.")]
        public static float extraShield = 60;

        [ConfigurableField(ConfigName = "Bonus Armor", ConfigDesc = "Amount of armor added when heart breaks.")]
        public static float heartArmor = 80;

        [ConfigurableField(ConfigName = "Bonus Armor Duration", ConfigDesc = "Length of the Heart's armor debuff.")]
        public static float heartArmorDur = 3f;

        [ConfigurableField(ConfigName = "Shield Gating", ConfigDesc = "Whether the Heart should block damage past the remaining shield when broken.")]
        public static bool shieldGating = true;
        

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<GuardiansHeartBehavior>(stack);
        }



        public class GuardiansHeartBehavior : CharacterBody.ItemBehavior, IOnIncomingDamageServerReceiver
        {
            public void Start()
            {
                body.levelMaxShield = (float)Math.Pow(extraShield, 1 / stack);
            }
            public float currentShield;
            private void FixedUpdate()
            {
                currentShield = body.healthComponent.shield;
            }
            public void OnIncomingDamageServer(DamageInfo damageInfo)
            {
                if (body.healthComponent.shield > 1f && damageInfo.damage >= body.healthComponent.shield)
                {
                    if (shieldGating == true)
                    {
                        damageInfo.damage = body.healthComponent.shield;
                    }
                    
                    if (heartArmor > 0f)
                    {
                        body.AddTimedBuff(GuardiansHeartBuff.buff, heartArmorDur);
                    }
                }
            }
        }
    }
}
