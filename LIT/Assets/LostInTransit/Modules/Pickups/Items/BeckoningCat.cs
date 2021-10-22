using RoR2;
using System.Collections.Generic;
using UnityEngine;
using Moonstorm;

namespace LostInTransit.Items
{
    public class BeckoningCat : ItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("BeckoningCat");

        public static string section;

        [ConfigurableField(ConfigName = "Base Drop Chance", ConfigDesc = "Base chance for Elites to drop an item.")]
        public static float baseChance = 4.5f;

        [ConfigurableField(ConfigName = "Stacking Drop Chance", ConfigDesc = "Added chance for Elites to drop an item per stack.")]
        public static float stackChance = 1.5f;

        [ConfigurableField(ConfigName = "Uncommon Item Chance", ConfigDesc = "Chance for Elites to drop an Uncommon (Green) item.")]
        public static float greenItemChance = 6f;

        [ConfigurableField(ConfigName = "Uncommon Item Stacking Chance", ConfigDesc = "Extra chance for Elites to drop an Uncommon (Green) item per stack.")]
        public static float greenItemStack = 1f;

        [ConfigurableField(ConfigName = "Rare Item Chance", ConfigDesc = "Chance for Elites to drop a Rare (Red) item.")]
        public static float redItemChance = 0.5f;

        [ConfigurableField(ConfigName = "Rare Item Stacking Chance", ConfigDesc = "Extra chance for Elites to drop a Rare (Red) item per stack.")]
        public static float redItemStack = 0.25f;

        [ConfigurableField(ConfigName = "Use Luck", ConfigDesc = "Whether Luck should be accounted for in all Beckoning Cat-related rolls.")]
        public static bool usesLuck = true;

        /*
        public override void Config()
        {
            section = "Item: " + ItemDef.name;
            baseChance = LITMain.config.Bind<float>(section, "Base Drop Chance", 4.5f, "Base chance for Elites to drop an item.").Value;
            stackChance = LITMain.config.Bind<float>(section, "Stacking Drop Chance", 1.5f, "Added chance for Elites to drop an item per stack.").Value;
            greenItemChance = LITMain.config.Bind<float>(section, "Uncommon Item Chance", 6f, "Chance for Beckoning Cat to drop an Uncommon item.").Value;
            greenItemStack = LITMain.config.Bind<float>(section, "Uncommon Item Stacking Chance", 1f, "Chance per stack for Beckoning Cat to drop an Uncommon item.").Value;
            redItemChance = LITMain.config.Bind<float>(section, "Rare Item Chance", 0.5f, "Chance for Beckoning Cat to drop a Rare item.").Value;
            redItemStack = LITMain.config.Bind<float>(section, "Rare Item Stacking Chance", 0.25f, "Chance per stack of Beckoning Cat to drop a Rare item.").Value;
            usesLuck = LITMain.config.Bind<bool>(section, "Use Luck", true, "Whether the luck stat should be considered for Beckoning Cat drops.").Value;
        }*/
        /*public override void DescriptionToken()
        {
            LITUtil.AddTokenToLanguage(ItemDef.descriptionToken,
                $"Elite monsters have a <style=cIsUtility>{baseChance}%</style> <style=cStack>(+{stackChance}% per stack)</style> chance to drop items on death. Dropped items have a <style=cIsUtility>{greenItemChance}%</style> <style=cStack>(+{greenItemStack}% per stack)</style> chance to be <color=#81d047>Uncommon</color>, and a <style=cIsUtility>{redItemChance}%</style> <style=cStack>(+{redItemStack}% per stack)</style> chance to be <color=#f26060>Rare</color>.",
                LangEnum.en);
        }*/
        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<BeckoningCatBehavior>(stack);
        }

        public class BeckoningCatBehavior : CharacterBody.ItemBehavior, IOnKilledOtherServerReceiver
        {
            public List<PickupIndex> redItems = Run.instance.availableTier3DropList;
            public List<PickupIndex> greenItems = Run.instance.availableTier2DropList;
            public List<PickupIndex> whiteItems = Run.instance.availableTier1DropList;

            private int nextRedItem;
            private int nextGreenItem;
            private int nextWhiteItem;

            private static readonly float Offset = 2f * Mathf.PI / Run.instance.participatingPlayerCount;
            private Vector3 constant = (Vector3.up * 20f) + (5 * Vector3.right * Mathf.Cos(Offset)) + (5 * Vector3.forward * Mathf.Sin(Offset));

            //Swuff's original code hurts me so i'm re-using the one from varianceAPI.
            //★ at least my code dropped items more than 0.9% of the time :smirk:
            //Yeah thats fair.
            //★ ily
            //Funnily enough this didn't work like in ror1 where it used every elite modifier, now it does, chad.
            public void OnKilledOtherServer(DamageReport damageReport)
            {
                RefreshNextItems();
                var victimBody = damageReport.victimBody;
                var dropLocation = damageReport.attackerBody.transform.position;
                for(int i = 0; i < victimBody.eliteBuffCount; i++)
                {
                    if (victimBody.isElite && Roll(baseChance + (stackChance * (stack - 1)), 0))
                    {
                        //Debug.Log("Rolling for item...");
                        var redItem = usesLuck ? Roll(redItemChance + (redItemStack * (stack - 1)), body.master.luck) : Roll(redItemChance + (redItemStack * (stack - 1)), 0);
                        //Debug.Log($"red Item? {redItem}");
                        if (redItem)
                        {

                            SpawnItem(redItems, nextRedItem);
                            return;
                        }
                        var greenItem = usesLuck ? Roll(greenItemChance + (greenItemStack * (stack - 1)), body.master.luck) : Roll(greenItemChance + (greenItemStack * (stack - 1)), 0);
                        //Debug.Log($"green Item? {greenItem}");
                        if (greenItem)
                        {
                            SpawnItem(greenItems, nextGreenItem);
                            return;
                        }
                        else
                        {
                            SpawnItem(whiteItems, nextWhiteItem);
                            return;
                        }
                    }

                }
                void SpawnItem(List<PickupIndex> items, int nextItem)
                {
                    PickupDropletController.CreatePickupDroplet(items[nextItem], victimBody.transform.position, constant);
                }
            }
            private void RefreshNextItems()
            {
                nextWhiteItem = Run.instance.treasureRng.RangeInt(0, whiteItems.Count);
                nextGreenItem = Run.instance.treasureRng.RangeInt(0, greenItems.Count);
                nextRedItem = Run.instance.treasureRng.RangeInt(0, redItems.Count);
            }


            private bool Roll(float chance, float usesLuck)
            {
                return Util.CheckRoll(chance, usesLuck);
            }
        }
    }
}