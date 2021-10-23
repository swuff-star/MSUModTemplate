using Moonstorm;
using RoR2;
using System;

namespace LostInTransit.Items
{
    public class SmartShopper : ItemBase
    {
        private const string token = "LIT_ITEM_SMARTSHOPPER_DESC";
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("SmartShopper");

        [ConfigurableField(ConfigName = "Money Bonus", ConfigDesc = "Amount of extra money gained, per stack.")]
        [TokenModifier(token, StatTypes.Percentage, 0)]
        public static float goldAmount = 0.25f;

        [ConfigurableField(ConfigName = "Use Exponential Scaling", ConfigDesc = "Whether scaling should be done exponentially or linearally.")]
        public static bool usesExpScaling = true;

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<SmartShopperBehavior>(stack);
        }

        public class SmartShopperBehavior : CharacterBody.ItemBehavior, IOnKilledOtherServerReceiver
        {
            public void OnKilledOtherServer(DamageReport damageReport)
            {
                var deathRewards = damageReport.victimBody.GetComponent<DeathRewards>();
                float smartShopperGold;
                if (deathRewards)
                {
                    //Debug.WriteLine("Gold before Smart Shopper: " + deathRewards.goldReward);
                    smartShopperGold = usesExpScaling ? (uint)(deathRewards.goldReward * Math.Pow(goldAmount, 1 / stack)) : (uint)(deathRewards.goldReward * goldAmount * stack);
                    //Debug.WriteLine("And that, times " + 0.25f * stack + "...");
                    //Debug.WriteLine("Comes out to " + smartShopperGold + " extra gold!");

                    body.master.GiveMoney((uint)smartShopperGold);
                }
            }
        }
    }
}
