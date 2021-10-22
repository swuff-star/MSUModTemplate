using RoR2;
using Moonstorm;
using System;

namespace LostInTransit.Items
{
    public class SmartShopper : ItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("SmartShopper");
        //public static string section;

        [ConfigurableField(ConfigName = "Money Bonus", ConfigDesc = "Amount of extra money gained, per stack.")]
        public static float goldAmount = 0.25f;

        [ConfigurableField(ConfigName = "Use Exponential Scaling", ConfigDesc = "Whether scaling should be done exponentially or linearally.")]
        public static bool usesExpScaling = true;

        /*
        public override void Config()
        {
            section = "Item: " + ItemDef.name;
            goldAmount = LITMain.config.Bind<float>(section, "Money Bonus", 0.25f, "Amount of extra money gained per stack.").Value;
            usesExpScaling = LITMain.config.Bind<bool>(section, "Use Exponential Scaling", true, "Whether scaling should be done exponentially (money bonus ^ (1 / stack)) or linearally (money bonus * stack).").Value;
        }*/

        /*
        public override void DescriptionToken()
        {
            LITUtil.AddTokenToLanguage(ItemDef.descriptionToken,
                $"Monsters drop <style=cIsUtility>{goldAmount * 100}%</style> <style=cStack>(+{goldAmount * 100}% per stack)</style> more gold.",
                LangEnum.en);
        }*/

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
                    smartShopperGold = usesExpScaling ? (uint)(deathRewards.goldReward * Math.Pow(goldAmount, 1/stack)) : (uint)(deathRewards.goldReward * goldAmount * stack);
                    //Debug.WriteLine("And that, times " + 0.25f * stack + "...");
                    //Debug.WriteLine("Comes out to " + smartShopperGold + " extra gold!");
                    
                    body.master.GiveMoney((uint)smartShopperGold);
                }
            }
        }
    }
}
