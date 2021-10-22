using RoR2;
using Moonstorm;
using System;
using UnityEngine;

namespace LostInTransit.Items
{
    public class RustyJetpack : ItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("RustyJetpack");

        [ConfigurableField(ConfigName = "Gravity Reduction", ConfigDesc = "Reduced gravity per Jetpack.")]
        public static float reducedGravity = 0.15f;

        [ConfigurableField(ConfigName = "Jump Power", ConfigDesc = "Added jump power per Jetpack.")]
        public static float addedJumpPower = 5f;

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<RustyJetpackBehavior>(stack);
        }

        public class RustyJetpackBehavior : CharacterBody.ItemBehavior, IStatItemBehavior
        {
            public void RecalculateStatsEnd()
            {
                body.jumpPower += addedJumpPower + ((addedJumpPower / 2) * (stack - 1));
            }

            public void RecalculateStatsStart()
            {
            }
            private void FixedUpdate()
            {
                float gravReduction = (float)Math.Pow(reducedGravity, 4 / stack + 3);
                body.characterMotor.velocity.y -= Time.fixedDeltaTime * Physics.gravity.y * gravReduction;
            }
            
        }
    }
}
