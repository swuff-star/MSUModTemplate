using RoR2;
using UnityEngine.Networking;
using Moonstorm;
using System;
using UnityEngine;

namespace LostInTransit.Items
{
    public class LifeSavings : ItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("LifeSavings");
        public static ItemDef itemDef;
        public static string section;
        public static float moneyKept;

        //★ (godzilla 1998 main character voice)
        //★ that's a lotta Debug.WriteLine()
        //Neb - Why dont use Debug.Log(), lol | Doesnt matter, i killed this code and rewrote it as my child.
        public override void Initialize()
        {
            itemDef = ItemDef;
            section = "Item: " + ItemDef.name; 
            moneyKept = LITMain.config.Bind<float>(section, "Money Kept", 4f, "Percentage of money kept between stages.").Value;
        }
        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<LifeSavingsBehavior>(stack);
        }

        public class LifeSavingsBehavior : CharacterBody.ItemBehavior
        {
            public LifeSavingsMasterBehavior MasterBehavior
            {
                get
                {
                    if (!_masterBehavior)
                    {
                        var component = body.master?.GetComponent<LifeSavingsMasterBehavior>();
                        if (component)
                        {
                            _masterBehavior = component;
                            return _masterBehavior;
                        }
                        else
                        {
                            _masterBehavior = body.master?.gameObject.AddComponent<LifeSavingsMasterBehavior>();
                            return _masterBehavior;
                        }
                    }
                    else
                        return _masterBehavior;
                }
            }

            private LifeSavingsMasterBehavior _masterBehavior;

            public void Start()
            {
                //Only add master behaviors to players.
                if(body.isPlayerControlled)
                    MasterBehavior.UpdateStacks();
            }

            private void OnDestroy()
            {
                if(body.isPlayerControlled)
                    MasterBehavior.CheckIfShouldDestroy();
            }
        }

        //This is one of the rare few cases where an item behavior is not enough.
        public class LifeSavingsMasterBehavior : MonoBehaviour
        {
            public CharacterMaster CharMaster { get => gameObject.GetComponent<CharacterMaster>(); }
            public int stack;
            public bool moneyPending;
            public uint storedGold;

            public void Start()
            {
                CharMaster.inventory.onInventoryChanged += UpdateStacks;
                SceneExitController.onBeginExit += ExtractMoney;
                Stage.onStageStartGlobal += GiveMoney;
            }

            internal void UpdateStacks()
            {
                stack = (int)CharMaster?.inventory.GetItemCount(itemDef);
            }

            private void ExtractMoney(SceneExitController obj)
            {
                moneyPending = true;
                storedGold = CalculatePercentage();
            }

            private uint CalculatePercentage()
            {
                uint toReturn;
                toReturn = (uint)(CharMaster.money / 100 * Mathf.Min(moneyKept * stack, 100));
                CharMaster.money -= toReturn;
                return toReturn;
            }
            private void GiveMoney(Stage obj)
            {
                CharMaster.GiveMoney(storedGold);
                storedGold = 0;
                moneyPending = false;
            }

            public void CheckIfShouldDestroy()
            {
                if (!moneyPending)
                    Destroy(this);
            }

            public void OnDestroy()
            {
                SceneExitController.onBeginExit -= ExtractMoney;
                Stage.onStageStartGlobal -= GiveMoney;
            }
        }
    }
}
