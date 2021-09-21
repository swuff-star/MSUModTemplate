using RoR2;
using Moonstorm;
using System.Diagnostics;

namespace LostInTransit.Items
{
    public class BitterRoot : ItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("BitterRoot");

        public static float rootIncrease;
        public static float rootCap;
        public override void Initialize()
        {
            rootIncrease = LITMain.config.Bind<float>("Item: " + ItemDef.name, "Extra Maximum Health Per Root", 0.04f, "Extra health added per root.").Value;
            rootCap = LITMain.config.Bind<float>("Item: " + ItemDef.name, "Maximum Health Gain", 0f, "Maximum health increase that can be obtained from roots, as a % (e.g. 300 = 300%). Set to 0 to disable the cap.").Value;
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
                float rootTotal = rootIncrease * stack;
                if (rootTotal > (rootCap / 100) && rootCap != 0) 
                { rootTotal = (rootCap / 100); }
                //Debug.WriteLine("rootTotal = " + rootTotal);
                float rootGain = rootTotal * (body.baseMaxHealth + (body.levelMaxHealth * body.level - 1));
                body.maxHealth += rootGain;
            }
        }
    }
}
