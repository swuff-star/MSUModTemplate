using LostInTransit.Buffs;
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

        [ConfigurableField(ConfigName = "Base duration of buff from Blessed Dice", ConfigDesc = "Base duration of buff after using a shrine.")]
        [TokenModifier(token, StatTypes.Default, 0)]
        public static float newBaseTimer = 10f;

        [ConfigurableField(ConfigName = "Duration of buff from Blessed Dice", ConfigDesc = "Added duration of buff per stack of Dice.")]
        [TokenModifier(token, StatTypes.Default, 1)]
        public static float newStackTimer = 5f;

        [ConfigurableField(ConfigName = "Healing from buff", ConfigDesc = "Healing per second while you have the heal buff, as a percentage of max health")]
        public static float healAmount = 2f;

        [ConfigurableField(ConfigName = "Armor from buff", ConfigDesc = "Armor added while you have the armor buff.")]
        public static float armorAmount = 50f;

        [ConfigurableField(ConfigName = "Move speed from buff", ConfigDesc = "Move speed added while you have the move speed buff, in percent.")]
        public static float moveAmount = 50f;

        [ConfigurableField(ConfigName = "Attack speed from buff", ConfigDesc = "Attack speed added while you have the attack speed buff, in percent.")]
        public static float atkAmount = 50f;

        [ConfigurableField(ConfigName = "Crit chance from buff", ConfigDesc = "Critical strike chance added while you have the critical strike buff, in percent.")]
        public static float critAmount = 20f;

        [ConfigurableField(ConfigName = "Luck from buff", ConfigDesc = "Luck added while you have the luck buff. (Whole numbers only)")]
        public static int luckAmount = 1;

        [ConfigurableField(ConfigName = "Weighted Rolls", ConfigDesc = "Make all buffs equally likely, instead of weighted for balance")]
        public static bool fairRolls = false;



        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<BlessedDiceBehavior>(stack);
        }

        public class BlessedDiceBehavior : CharacterBody.ItemBehavior
        {
            private float CalcBuffTimer()
            {
                float stackTimer = newStackTimer * (stack - 1);
                return newBaseTimer + stackTimer;
            }
            
            private BuffDef ChooseRandomBuff()
            {
                int weight = 7;
                if (!fairRolls)
                { 
                    weight += 5;
                }
                BuffDef buff = null;
                int rng = Run.instance.runRNG.RangeInt(1, weight);
                switch (rng)
                {
                    case 1:
                        buff = DiceLuck.buff;
                        break;
                    case 2:
                    case 7:
                        buff = DiceCrit.buff;
                        break;
                    case 3:
                    case 8:
                        buff = DiceAtk.buff;
                        break;
                    case 4:
                    case 9:
                        buff = DiceMove.buff;
                        break;
                    case 5:
                    case 10:
                        buff = DiceArmor.buff;
                        break;
                    case 6:
                    case 11:
                        buff = DiceRegen.buff;
                        break;
                }
                return buff;
            }
            
            private void AddBuffOnShrine(Interactor interactor, IInteractable interactable, GameObject interactableObject)
            {
                MonoBehaviour monoBehaviour = (MonoBehaviour)interactable;
                bool isPurchase = monoBehaviour.GetComponent<PurchaseInteraction>();
                //These have to be nested to prevent NREs when checking for isShrine on non-purchase events
                if (isPurchase)
                {
                    if (interactableObject.GetComponent<PurchaseInteraction>().isShrine)
                    {
                        body.AddTimedBuff(ChooseRandomBuff(), CalcBuffTimer());
                    }
                }
            }

            public void Start()
            {
                GlobalEventManager.OnInteractionsGlobal += AddBuffOnShrine;
            }
            
            public void OnDestroy()
            {
                GlobalEventManager.OnInteractionsGlobal -= AddBuffOnShrine;
            }
        }
    }
}
