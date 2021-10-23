using Moonstorm;
using RoR2;
using UnityEngine;

namespace LostInTransit.Items
{
    public class LifeSavings : ItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("LifeSavings");
        public static ItemDef itemDef;

        [ConfigurableField(ConfigName = "Money Kept Between Stages", ConfigDesc = "Percentage of money kept between stages")]
        public static float newMoneyKeptBase = 5f;

        [ConfigurableField(ConfigName = "Extra Money Kept Per Stack", ConfigDesc = "Extra percentage of money kept between stages for each stack.")]
        public static float newMoneyKeptStack = 2.5f;

        /*
        public override void Config()
        {
            var section = $"Item: {ItemDef.name}";
            newMoneyKeptBase = LITMain.config.Bind<float>(section, "Money Kept", 5f, "Percentage of money kept between stages.").Value;
            newMoneyKeptStack = LITMain.config.Bind<float>(section, "Money Kept per Stack", 2.5f, "Amount of kept money added for each stack of Life Savings").Value;
        }*/

        /*
        public override void DescriptionToken()
        {
            LITUtil.AddTokenToLanguage(ItemDef.descriptionToken,
                $"Keep <style=cIsUtility>{newMoneyKeptBase}%</style> <style=cStack>(+{newMoneyKeptStack}% per stack)</style> of <style=cIsUtility>earned gold</style> between stages. Gold is not kept when travelling between <style=cWorldEvent>Hidden Realms</style>.",
                LangEnum.en);
        }*/

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
                        else if (body.master?.playerCharacterMasterController != null)
                        {
                            _masterBehavior = body.master?.gameObject.AddComponent<LifeSavingsMasterBehavior>();
                            return _masterBehavior;
                        }
                        return _masterBehavior;
                    }
                    else
                        return _masterBehavior;
                }
            }

            private LifeSavingsMasterBehavior _masterBehavior;

            public void Start()
            {
                //Only add master behaviors to players.
                if (body.isPlayerControlled)
                    MasterBehavior.UpdateStacks();
            }

            private void OnDestroy()
            {
                if (body.isPlayerControlled)
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
                UpdateStacks();
            }

            internal void UpdateStacks()
            {
                stack = (int)CharMaster?.inventory.GetItemCount(itemDef);
            }

            private void ExtractMoney(SceneExitController obj)
            {
                if ((bool)!Run.instance?.isRunStopwatchPaused)
                {
                    moneyPending = true;
                    storedGold = CalculatePercentage();
                }
            }

            private uint CalculatePercentage()
            {
                var percentage = newMoneyKeptBase + (newMoneyKeptStack * (stack - 1));

                uint toReturn;
                toReturn = (uint)(CharMaster.money / 100 * Mathf.Min(percentage, 100));
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
                CharMaster.inventory.onInventoryChanged -= UpdateStacks;
            }
        }
    }
}
