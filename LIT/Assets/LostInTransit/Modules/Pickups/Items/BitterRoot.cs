using RoR2;
using Moonstorm;
using UnityEngine;

namespace LostInTransit.Items
{
    public class BitterRoot : LITItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("BitterRoot");

        public static float rootIncrease;
        public static float rootCap;
        public override void Initialize()
        {
            Config();
            DescriptionToken();
        }
        public override void Config()
        {
            var section = $"Item: {ItemDef.name}";
            rootIncrease = LITMain.config.Bind<float>(section, "Extra Maximum Health Per Root", 4f, "Extra health percentage added per root.").Value;
            rootCap = LITMain.config.Bind<float>(section, "Maximum Health Gain", 0f, "Maximum health increase that can be obtained from roots, as a % (e.g. 300 = 300%). Set to 0 to disable the cap.").Value;
        }
        public override void DescriptionToken()
        {
            LITUtil.AddTokenToLanguage(ItemDef.descriptionToken,
                $"Increase <style=cIsHealing>maximum health</style> by <style=cIsHealing>+{rootIncrease}%</style> <style=cStack>(+{rootIncrease}% per stack)</style>",
                LangEnum.en);
        }
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

                float rootGain = rootTotal * (body.baseMaxHealth + (body.levelMaxHealth * body.level - 1));
                body.maxHealth += rootGain;
            }
        }
    }
}
