using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostInTransit.Items
{
    public class BeckoningCat : ItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("BeckoningCat");

        public static string section;
        public static float baseChance;
        public static float stackChance;
        public static float greenItemChance;
        public static float greenItemStack;
        public static float redItemChance;
        public static float redItemStack;
        public static bool usesLuck;

        public override void Initialize()
        {
            section = "Item: " + ItemDef.name;
            baseChance = LITMain.config.Bind<float>(section, "Base Drop Chance", 4.5f, "Base Chance for Elites droping an item while having this item.").Value;
            stackChance = LITMain.config.Bind<float>(section, "Stacking Drop Chance", 1.5f, "Value added to the base chance per stack.").Value;
            greenItemChance = LITMain.config.Bind<float>(section, "Uncommon Item Chance", 6f, "Chance for beckoning cat to drop an Uncommon item.").Value;
            greenItemStack = LITMain.config.Bind<float>(section, "Uncommon Item Stacking Chance", 1f, "Chance per stack for Beckoning Cat to drop an Uncommon item").Value;
            redItemChance = LITMain.config.Bind<float>(section, "Rare Item Chance", 0.5f, "Chance for beckoning cat to drop a Rare item.").Value;
            redItemStack = LITMain.config.Bind<float>(section, "Rare Item Stacking Chance", 0.25f, "Chance per stack of Beckoning Cat to drop a Rare item").Value;
            usesLuck = LITMain.config.Bind<bool>(section, "Use Luck", true, "Wether or not Beckoning cat will use luck in its calculations.").Value;
        }
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
            public void OnKilledOtherServer(DamageReport damageReport)
            {
                RefreshNextItems();
                var victimBody = damageReport.victimBody;
                if(victimBody.isElite && Roll(baseChance + (stackChance * (stack - 1)), 0))
                {
                    //Debug.Log("Rolling for item...");
                    var redItem = usesLuck ? Roll(redItemChance + (redItemStack * (stack - 1)), body.master.luck) : Roll(redItemChance + (redItemStack * (stack - 1)), 0);
                    //Debug.Log($"red Item? {redItem}");
                    if(redItem)
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
            private void RefreshNextItems()
            {
                nextWhiteItem = Run.instance.treasureRng.RangeInt(0, whiteItems.Count);
                nextGreenItem = Run.instance.treasureRng.RangeInt(0, greenItems.Count);
                nextRedItem = Run.instance.treasureRng.RangeInt(0, redItems.Count);
            }

            private void SpawnItem(List<PickupIndex> items, int nextItem)
            {
                PickupDropletController.CreatePickupDroplet(items[nextItem], body.transform.position, constant);
            }

            private bool Roll(float chance, float usesLuck)
            {
                return Util.CheckRoll(chance, usesLuck);
            }
        }
    }
}