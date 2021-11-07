using Moonstorm;
using RoR2;
using UnityEngine;

namespace LostInTransit.Items
{
    [DisabledContent]
    public class BitterRoot : ItemBase
    {
        private const string token = "LIT_ITEM_BITTERROOT_DESC";
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("BitterRoot");

        [ConfigurableField(ConfigName = "Extra Maximum Health per Root", ConfigDesc = "Extra percentage of maximum health added per root")]
        [TokenModifier(token, StatTypes.Default)]
        public static float rootIncrease = 4f;

        [ConfigurableField(ConfigName = "Maximum Health Gain", ConfigDesc = "Maximum health increase that can be obtained from roots, as a % (e.g. 300 = 300%). Set to 0 to disable.")]
        public static float rootCap = 0f;

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<BitterRootBehavior>(stack);
        }

        //I had a go at making this not lose HP when sprinting but it didn't work -g

        public class BitterRootBehavior : CharacterBody.ItemBehavior, IStatItemBehavior
        {
            private float rootGain = 0f;
            private bool statsDirty = false;
            private float hpTracker = 0f;
            private int stackTracker = 0;
            public void RecalculateStatsStart() 
            {
                if (statsDirty && stackTracker >= stack)
                {
                    body.maxHealth += rootGain;
                }
            }
            public void RecalculateStatsEnd()
            {
                if (hpTracker + rootGain != body.maxHealth || stackTracker != stack)
                {
                    statsDirty = false;
                }
                if (!statsDirty)
                {
                    statsDirty = true;
                    hpTracker = body.maxHealth;
                    stackTracker = stack;
                    
                    float rootTotal = 0;
                    if (rootCap > 0)
                        rootTotal = Mathf.Min(rootIncrease * stack, rootCap);
                    else
                        rootTotal = rootIncrease * stack;
                    rootGain = (rootTotal / 100) * (body.baseMaxHealth + (body.levelMaxHealth * (body.level - 1)));
                    body.maxHealth += rootGain;
                }
            }
        }
    }
}
