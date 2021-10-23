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
            body.AddItemBehavior<MysteriousVialBehavior>(stack);
        }

        public class MysteriousVialBehavior : CharacterBody.ItemBehavior, IStatItemBehavior
        {
            public void RecalculateStatsStart() { }
            public void RecalculateStatsEnd()
            {
                float rootTotal = 0;
                if (rootCap > 0)
                    rootTotal = Mathf.Min(rootIncrease * stack, rootCap);
                else
                    rootTotal = rootIncrease * stack;

                float rootGain = (rootTotal / 100) * (body.baseMaxHealth + (body.levelMaxHealth * (body.level - 1)));
                body.maxHealth += rootGain;
            }
        }
    }
}
