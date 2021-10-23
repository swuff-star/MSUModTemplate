using Moonstorm;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace LostInTransit.Items
{
    public class BeckoningCat : ItemBase
    {
        private const string token = "LIT_ITEM_BECKONINGCAT_DESC";

        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("BeckoningCat");

        public static string section;

        [ConfigurableField(ConfigName = "Base Drop Chance", ConfigDesc = "Base chance for Elites to drop an item.")]
        [TokenModifier(token, StatTypes.Default, 0)]
        public static float baseChance = 4.5f;

        [ConfigurableField(ConfigName = "Stacking Drop Chance", ConfigDesc = "Added chance for Elites to drop an item per stack.")]
        [TokenModifier(token, StatTypes.Default, 1)]
        public static float stackChance = 1.5f;

        [ConfigurableField(ConfigName = "Uncommon Item Chance", ConfigDesc = "Chance for Elites to drop an Uncommon (Green) item.")]
        [TokenModifier(token, StatTypes.Default, 2)]
        public static float greenItemChance = 6f;

        [ConfigurableField(ConfigName = "Uncommon Item Stacking Chance", ConfigDesc = "Extra chance for Elites to drop an Uncommon (Green) item per stack.")]
        [TokenModifier(token, StatTypes.Default, 3)]
        public static float greenItemStack = 1f;

        [ConfigurableField(ConfigName = "Rare Item Chance", ConfigDesc = "Chance for Elites to drop a Rare (Red) item.")]
        [TokenModifier(token, StatTypes.Default, 4)]
        public static float redItemChance = 0.5f;

        [ConfigurableField(ConfigName = "Rare Item Stacking Chance", ConfigDesc = "Extra chance for Elites to drop a Rare (Red) item per stack.")]
        [TokenModifier(token, StatTypes.Default, 5)]
        public static float redItemStack = 0.25f;

        [ConfigurableField(ConfigName = "Use Luck", ConfigDesc = "Whether Luck should be accounted for in all Beckoning Cat-related rolls.")]
        public static bool usesLuck = true;

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
                for (int i = 0; i < victimBody.eliteBuffCount; i++)
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