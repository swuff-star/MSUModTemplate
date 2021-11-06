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
                int rng = Run.instance.runRNG.RangeInt(1, 13);
                switch (rng)
                {
                    //cases look stupid
                    //Luck is 1/12 chance
                    //heal is 3/12 chance
                    //all other buffs are 2/12 chance
                    case < 2:
                        Debug.Log("luck");
                        break;
                    case < 4:
                        Debug.Log("crit");
                        break;
                    case < 6:
                        Debug.Log("atkspd");
                        break;
                    case < 8:
                        Debug.Log("move");
                        break;
                    case < 10:
                        Debug.Log("armor");
                        break;
                    case < 13:
                        Debug.Log("heal");
                        break;
                }
                return rng;
            }
           public void Start()
           {
                //I need to encapsulate this properly so I can onDestroy it
                GlobalEventManager.OnInteractionsGlobal += delegate (Interactor interactor, IInteractable interactable, GameObject interactableObject)
                {
                    //Need more NRE checks - currently throws an NRE on non-chest interactables and on items
                    if (interactableObject.GetComponent<PurchaseInteraction>().isShrine)
                    {
                        ChooseRandomBuff();
                        Debug.Log(stack);
                    }
                    else
                    {
                        Debug.Log("please no NRE");
                    }
                };
           }
        }
    }
}
