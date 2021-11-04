using R2API;
using UnityEngine;
using UnityEngine.Networking;
using Moonstorm;
using RoR2;
using LostInTransit.Components;

namespace LostInTransit.Items
{
    public class MeatNugget : ItemBase
    {
        private const string token = "LIT_ITEM_MEATNUGGET_DESC";
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("MeatNugget");

        public static GameObject MeatNuggetPickup = Assets.LITAssets.LoadAsset<GameObject>("MeatNuggetPickup");

        [ConfigurableField(ConfigName = "Proc Chance", ConfigDesc = "Base proc chance for Meat Nugget.")]
        [TokenModifier(token, StatTypes.Default, 0)]
        public static float newBaseChance = 8f;

        [ConfigurableField(ConfigName = "Proc Chance per Stack", ConfigDesc = "Extra proc chance per stack of Meat Nugget.")]
        [TokenModifier(token, StatTypes.Default, 1)]
        public static float newStackChance = 0f;

        [ConfigurableField(ConfigName = "Regen Multiplier", ConfigDesc = "Multiplier added to regen by nugget pickup")]
        [TokenModifier(token, StatTypes.Percentage, 0)]
        public static float regenMultiplier = 0.5f;

        [ConfigurableField(ConfigName = "Does Regen Stack", ConfigDesc = "If true, the regen buff can stack up to the number of Meat Nuggets you have")]
        public static bool doesStack = true;

        [ConfigurableField(ConfigName = "Duration", ConfigDesc = "Base duration of the regen buff granted by dropped nuggets.")]
        [TokenModifier(token, StatTypes.Default, 1)]
        public static float newBaseDuration = 2;

        [ConfigurableField(ConfigName = "Stacking Duration", ConfigDesc = "Extra duration of the regen buff per stack of Meat Nugget.")]
        [TokenModifier(token, StatTypes.Default, 2)]
        public static float newStackDuration = 0;

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<MeatNuggetBehavior>(stack);
        }
        public class MeatNuggetBehavior : CharacterBody.ItemBehavior, IOnDamageDealtServerReceiver
        {
            public void OnDamageDealtServer(DamageReport damageReport)
            {
                GameObject victim = damageReport.victim.gameObject;
                if (Util.CheckRoll(CalcChance() * damageReport.damageInfo.procCoefficient, damageReport.attackerMaster))
                {
                    GameObject nugget = UnityEngine.Object.Instantiate<GameObject>(MeatNuggetPickup, victim.transform.position, UnityEngine.Random.rotation);
                    nugget.GetComponent<TeamFilter>().teamIndex = damageReport.attackerTeamIndex;
                    NuggetPickup nugbuff = nugget.GetComponentInChildren<NuggetPickup>();
                    nugbuff.BuffTimer = CalcDuration();
                    nugbuff.RegenMult = regenMultiplier;
                    
                    if (doesStack) 
                        nugbuff.RegenStacks = stack;

                    NetworkServer.Spawn(nugget);
                }
            }
            private float CalcChance()
            {
                float stackChance = newStackChance * (stack - 1);
                return newBaseChance + stackChance;
            }
            private float CalcDuration()
            {
                float stackDuration = newStackDuration * (stack - 1);
                return newBaseDuration + stackDuration;
            }
        }
    }
}
