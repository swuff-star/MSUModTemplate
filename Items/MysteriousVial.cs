using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityEngine;
using MonoMod.Cil;
using static LostInTransit.LostInTransitMain;

namespace LostInTransit.Items
{
    public class MysteriousVial : ItemBase
    {
        public override string ItemName => "Mysterious Vial";

        public override string ItemLangTokenName => "MYSTERIOUS_VIAL";

        public override string ItemPickupDesc => "Regenerate more health.";

        public override string ItemFullDescription => $"Regenerate an extra <style=cIsHealing>+{MysteriousVial.vialRegen}</style> <style=cStack>(+{MysteriousVial.vialRegen} per stack)</style> <style=cIsHealing>hp</style> per second.";

        public override string ItemLore => "Order: Experimental Medicine\n" +
"Tracking Number: 951*****\n" +
"Estimated Delivery: 8/26/2056\n" +
"Shipping Method: High Priority / Fragile\n" +
"Shipping Address: Asklepios Medical Labs, Tranquility Base, Luna\n\n" +

"Greetings! I know you guys are still working on that cure, and I think I've finally figured something out. Well, part of something.\n\n" +

"I've been running it on rats, and it's worked with a 99.3% success rate. Cured! Completely! But, uh, there's been some less than desirable side effects... I'm hoping by sending you guys a sample, you guys can try to figure something out. As it is now, it uses gene therapy to create a complete immunity, and I've even included the formula I've used to make the stuff. That said, here's a short list of things to look out for:\n\n" +

"First off, just a few general sicknesses things - nothing to worry about. Fever, chills, nausea, stuff that the body can overcome. Some have also caused a bit of mental side effects, including what I think is - but obviously, can't tell completely with the rats - a dementia / alzheimers like effect. Some also have shown signs of a, like... \"rat anxiety\". Super antisocial, scared when being fed, lots of weird stuff. Some even have these lil' manic episodes where they run around their cages. I know you guys generally check for withdrawal symptoms, and I've done the same. They're present, and uh, very, very prominent. Remember that pill Böhm put out, the one that was supposed to prevent headaches? And the following lawsuits? Triple that. But you guys are geniuses, I'm sure that can be worked out. Oh, I should probably add there's been a few organ failures and 'modifications' also with some tests... Some liver failures, some encephalitis, some kidney failures, pretty much everything wrong with the heart from angina to straight up failure. Bone decalcification should definitely be looked at, and the paralysis some have experienced also probably isn't the best... If I'm being completely honest, I think I might've invented a few new side effects myself with this whole thing. Corrosive blood, tooth loss, fungal growths, joint dislocations, flashbacks (some rats showed symptoms similar to PTSD in response to other pills), literacy (I caught a rat staring at this letter for a bit too long as I wrote it, this is my best guess as to why), forceful explulsion of urine (0.9 - 1.2 PSI), intangibility (one rat stole food straight out of another's mouth - straight through the face), losing sense of touch, losing sense of time, flashforwards (some rats learned things I hadn't planned to teach them until later), anxiety relief (maybe look into this one more?), and revirginification (no comment). Hopefully this isn't too much and you guys can figure all this out! Can't wait to hear back, this one could definitely be the one.";

        public override ItemTier Tier => ItemTier.Tier1;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("Vial.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("Vial.png");

        public static float vialRegen;
        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateItem();
            Hooks();
        }
        public void CreateConfig(ConfigFile config)
        {
            vialRegen = config.Bind<float>("Item: " + ItemName, "Extra Regen Per Vial", 1.2f, "Extra health regeneration added per item.").Value;
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }

        public override void Hooks()
        {
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }
        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            int vialCount = GetCount(self);
            self.regen += ((self.baseRegen + self.levelRegen * (self.level - 1))) + (vialRegen * vialCount);
        }
    }
}
