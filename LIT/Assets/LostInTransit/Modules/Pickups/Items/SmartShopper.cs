using LostInTransit.Modules;
using RoR2;
using System.Diagnostics;

namespace LostInTransit.Items
{
    public class SmartShopper : ItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("SmartShopper");
        public override void Initialize()
        {

        }
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
                    Debug.WriteLine("Gold before Smart Shopper: " + deathRewards.goldReward);
                    smartShopperGold = (uint)(deathRewards.goldReward * (stack * 0.25f));
                    Debug.WriteLine("And that, times " + 0.25f * stack + "...");
                    Debug.WriteLine("Comes out to " + smartShopperGold + " extra gold!");
                    body.master.GiveMoney((uint)smartShopperGold);
                }
            }
        }
    }
}
