using RoR2;
using Moonstorm;
using System.Diagnostics;
using LostInTransit.Buffs;
using UnityEngine.Networking;
using UnityEngine;

namespace LostInTransit.Items
{
    public class RepulsionArmor : LITItemBase
    {
        public override ItemDef ItemDef { get; set; } = Assets.LITAssets.LoadAsset<ItemDef>("RepulsionChestplate");

        public static float hitsNeededConfig;
        public static float hitsNeededConfigStack;
        public static float damageResist;
        public static float buffBaseLength;
        public static float buffStackLength;
        public static float durCap;
        public override void Initialize()
        {
            Config();
            DescriptionToken();
        }
        public override void Config()
        {
            var section = $"Item: {ItemDef.name}";
            hitsNeededConfig = LITMain.config.Bind<float>(section, "Hits Needed to Activate", 6f, "Amount of times to be hit to activate Repulsion Armor.").Value;
            hitsNeededConfigStack = LITMain.config.Bind<float>(section, "Hits Needed per Stack", 0f, "Amount of extra hits needed per stack to activate Repulsion Armor.").Value; //I don't think anyone wants this, but easy to implement and better safe than sorry.
            buffBaseLength = LITMain.config.Bind<float>(section, "Base Duration of Buff", 3f, "Amount of time the buff lasts with one stack.").Value;
            buffStackLength = LITMain.config.Bind<float>(section, "Stacking Duration of Buff", 1.5f, "Amount of time added to the Repulsion Armor buff per extra stack.").Value;
            durCap = LITMain.config.Bind<float>(section, "Maximum Duration", 0f, "Maximum length of the Repulsion Armor buff. Set to 0 to disable.").Value;
            damageResist = LITMain.config.Bind<float>(section, "Damage Reduction", 83f, "Amount of damage reduced while the Repulsion Armor buff is active, as a percent.").Value;
        }

        public override void DescriptionToken()
        {
            LITUtil.AddTokenToLanguage(ItemDef.descriptionToken,
                $"After <style=cIsUtility>taking damage</style> {hitsNeededConfig} times, gain a temporary <style=cIsUtility>{damageResist}% damage reduction</style> buff for <style=cIsUtility>{buffBaseLength}</style> <style=cStack>(+{buffStackLength} per stack)</style> seconds.",
                LangEnum.en);
        }

        public override void AddBehavior(ref CharacterBody body, int stack)
        {
            body.AddItemBehavior<RepulsionArmorBehavior>(stack);
        }

        public class RepulsionArmorBehavior : CharacterBody.ItemBehavior, IOnIncomingDamageServerReceiver
        {
            public float hitsNeededToActivate; //it's a mouthful but I am very high and I will easily be able to remember what it is for like this.
            private float stopwatch;
            private static float checkTimer = 0.25f;
            public void Awake()
            {
                stopwatch = 0f;
                hitsNeededToActivate = hitsNeededConfig + (hitsNeededConfigStack * (stack - 1));
                body.AddBuff(RepulsionArmorCD.buff);
                if (hitsNeededToActivate < 1)
                { hitsNeededToActivate = 1; } //Failsafe for if someone tries to set this shit to 0.
            }
            private void FixedUpdate()
            {
                if (NetworkServer.active)
                {
                    stopwatch += Time.fixedDeltaTime;
                    if (stopwatch > checkTimer)
                    {
                        stopwatch -= checkTimer;
                        //System.Diagnostics.Debug.WriteLine("It's been " + checkTimer + "!");
                        float currentCDBuffs = body.GetBuffCount(RepulsionArmorCD.buff);
                        float repulCount = (buffBaseLength + (buffStackLength * (stack - 1)));
                        if (repulCount > durCap && durCap != 0f)
                        { repulCount = durCap; }
                        if (currentCDBuffs < hitsNeededToActivate)
                        { body.AddBuff(RepulsionArmorCD.buff); } //Maybe use a bool alongside this to interact proper w/ Blast Shower? Might not be needed.
                        if (currentCDBuffs > hitsNeededToActivate && currentCDBuffs > 0) //There simply MUST be a better way to do this.
                        { body.RemoveBuff(RepulsionArmorCD.buff); }
                        if (currentCDBuffs == 0)
                        { 
                            body.AddTimedBuff(RepulsionArmorActive.buff, repulCount);
                            //hitsNeededToActivate = hitsNeededConfig; //I'm honestly not the happiest about this, but it gets the job done.
                            stopwatch -= repulCount + 0.05f;
                            //This SHOULD be a sneaky way to add a buffer to the cooldown. Could do this better by actually checking # of buffs. To-do: That, later.
                        }
                        if (body.HasBuff(RepulsionArmorActive.buff))
                        { hitsNeededToActivate = hitsNeededConfig; } //...This isn't exactly the result I'm looking for with the above, but who am I to complain about working code? Should probably make a new timer for it.
                    }
                } //That's a lotta if statements. Cleaner implementation probably possible but not worth pursuing at this time.
            }
            public void OnIncomingDamageServer(DamageInfo damageInfo)
            {
                hitsNeededToActivate -= 1f;
            }
        }
    }
}
