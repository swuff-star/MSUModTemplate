using Moonstorm;
using RoR2;
using UnityEngine;

namespace LostInTransit.Items
{
    [DisabledContent]
    public class BlessedDice : ItemBase
    {
        private const string token = "LIT_ITEM_BLESSEDDICE_DESC";
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("BlessedDice");

        [ConfigurableField(ConfigName = "Duration of buff from Blessed Dice", ConfigDesc = "Amount of time buffs last after using a shrine")]
        [TokenModifier(token, StatTypes.Default, 0)]
        [TokenModifier(token, StatTypes.DivideBy2, 1)]
        public static float newBuffTimer = 10f;

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<BlessedDiceBehavior>(stack);
        }

        public class BlessedDiceBehavior : CharacterBody.ItemBehavior
        {
            private int ChooseRandomBuff()
            {
                int rng = Run.instance.runRNG.RangeInt(1, 7);
                switch (rng)
                {
                    //cases look stupid
                    //Luck is 1/12 chance
                    //heal is 3/12 chance
                    //all other buffs are 2/12 chance
                    case 1:
                        Debug.Log("luck");
                        break;
                    case 2:
                        Debug.Log("crit");
                        break;
                    case 3:
                        Debug.Log("atkspd");
                        break;
                    case 4:
                        Debug.Log("move");
                        break;
                    case 5:
                        Debug.Log("armor");
                        break;
                    case 6:
                        Debug.Log("heal");
                        break;
                }
                return rng;
            }
            
            private void AddBuffOnShrine(Interactor interactor, IInteractable interactable, GameObject interactableObject)
            {
                //All these NRE checks adapted from Mystic's Items GenericGameEvents code
                MonoBehaviour monoBehaviour = (MonoBehaviour)interactable;

                bool isItem = monoBehaviour.GetComponent<GenericPickupController>();
                bool isVehicle = monoBehaviour.GetComponent<VehicleSeat>();
                bool isNetworkThingy = monoBehaviour.GetComponent<NetworkUIPromptController>();
                bool isShrine = interactableObject.GetComponent<PurchaseInteraction>().isShrine;
                bool allowProc = interactableObject.GetComponent<InteractionProcFilter>().shouldAllowOnInteractionBeginProc;

                if (!isItem && !isVehicle && !isNetworkThingy && isShrine && allowProc)
                {
                    ChooseRandomBuff();
                    Debug.Log(stack);
                }
                else
                {
                    Debug.Log("Not a Shrine");
                }
            }

            public void Start()
            {
                GlobalEventManager.OnInteractionsGlobal += AddBuffOnShrine;
            }
            
            public void onDestroy()
            {
                GlobalEventManager.OnInteractionsGlobal -= AddBuffOnShrine;
            }
        }
    }
}
