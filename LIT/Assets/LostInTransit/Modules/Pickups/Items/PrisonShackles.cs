using LostInTransit.Buffs;
using Moonstorm;
using RoR2;

namespace LostInTransit.Items
{
    public class PrisonShackles : ItemBase
    {
        private const string token = "LIT_ITEM_PRISONSHACKLES_DESC";
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("PrisonShackles");

        public static string section;
        [ConfigurableField(ConfigName = "Slow Multiplier", ConfigDesc = "Multiplier added to the shackled body's movement speed.")]
        [TokenModifier(token, StatTypes.Percentage, 0)]
        public static float slowMultiplier = 0.6f;

        [ConfigurableField(ConfigName = "Duration", ConfigDesc = "Base duration of the Shackled debuff.")]
        [TokenModifier(token, StatTypes.Default, 1)]
        public static int duration = 2;

        [ConfigurableField(ConfigName = "Stacking Duration", ConfigDesc = "Extra duration of the Shackled debuff per stack of shackles.")]
        [TokenModifier(token, StatTypes.Default, 2)]
        public static int durationStack = 2;

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<PrisonShacklesBehavior>(stack);
        }

        public class PrisonShacklesBehavior : CharacterBody.ItemBehavior, IOnDamageDealtServerReceiver
        {
            public void OnDamageDealtServer(DamageReport damageReport)
            {
                damageReport.victimBody.AddTimedBuff(Shackled.buff, duration + durationStack * (stack - 1));
            }
        }
    }
}
