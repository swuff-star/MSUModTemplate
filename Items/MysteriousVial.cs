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

        public static GameObject ItemDisplayModel => MainAssets.LoadAsset<GameObject>("VialDisplay.prefab");
        

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("Vial.png");

        public static GameObject ItemBodyModelPrefab;

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
            vialRegen = config.Bind<float>("Item: " + ItemName, "Extra Regen Per Vial", 0.4f, "Extra health regeneration added per item.").Value;
        }
        public static CharacterModel.RendererInfo[] ItemDisplaySetup(GameObject obj)
        {
            MeshRenderer[] meshes = obj.GetComponentsInChildren<MeshRenderer>();
            CharacterModel.RendererInfo[] renderInfos = new CharacterModel.RendererInfo[meshes.Length];

            for (int i = 0; i < meshes.Length; i++)
            {
                renderInfos[i] = new CharacterModel.RendererInfo
                {
                    defaultMaterial = meshes[i].material,
                    renderer = meshes[i],
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false //We allow the mesh to be affected by overlays like OnFire or PredatoryInstinctsCritOverlay.
                };
            }

            return renderInfos;
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemBodyModelPrefab = ItemDisplayModel;
            var itemDisplay = ItemBodyModelPrefab.AddComponent<ItemDisplay>();
            itemDisplay.rendererInfos = ItemDisplaySetup(ItemBodyModelPrefab);

            ItemDisplayRuleDict rules = new ItemDisplayRuleDict(new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Chest",
                    localPos = new Vector3(0, 0, 0),
                    localAngles = new Vector3(0, 0, 0),
                    localScale = new Vector3(0, 0, 0)
                }
            });
            //Base rules as nothing so I don't have to ever put up with "These displays are too big!" :)

            rules.Add("mdlCommandoDualies", new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
childName = "Chest",
localPos = new Vector3(0.1382F, 0.3761F, -0.1333F),
localAngles = new Vector3(359.3182F, 47.021F, 327.9326F),
localScale = new Vector3(0.06F, 0.06F, 0.06F)
                }
            });
            rules.Add("mdlHuntress", new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                   childName = "UpperArmR",
localPos = new Vector3(-0.0016F, 0.0545F, -0.0133F),
localAngles = new Vector3(276.3827F, 346.7739F, 17.9916F),
localScale = new Vector3(0.06F, 0.06F, 0.06F)
                }
            });
            rules.Add("mdlBandit2", new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "SideWeapon",
localPos = new Vector3(-0.0021F, -0.2926F, 0.1354F),
localAngles = new Vector3(64.6978F, 292.6223F, 264.1735F),
localScale = new Vector3(0.03F, 0.03F, 0.03F)
                }
            });
            rules.Add("mdlToolbot", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                   childName = "Head",
localPos = new Vector3(0.8788F, -0.273F, 1.0207F),
localAngles = new Vector3(56.4801F, 128.8035F, 101.2656F),
localScale = new Vector3(0.6F, 0.6F, 0.6F)
                }
});
            rules.Add("mdlEngi", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "CannonHeadR",
localPos = new Vector3(-0.1903F, 0.2231F, 0.1515F),
localAngles = new Vector3(0.3216F, 52.2449F, 79.3782F),
localScale = new Vector3(0.06F, 0.06F, 0.06F)
                }
});
            rules.Add("mdlMage", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "LowerArmL",
localPos = new Vector3(0.0052F, 0.1642F, -0.0937F),
localAngles = new Vector3(315.8914F, 292.4359F, 82.3083F),
localScale = new Vector3(0.06F, 0.06F, 0.06F)
                }
});
            rules.Add("mdlMerc", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "UpperArmL",
localPos = new Vector3(0.005F, -0.0866F, -0.0295F),
localAngles = new Vector3(318.7834F, 173.0106F, 149.0943F),
localScale = new Vector3(0.06F, 0.06F, 0.06F)
                }
});
            rules.Add("mdlTreebot", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
childName = "FlowerBase",
localPos = new Vector3(0F, 0.5193F, -0.8265F),
localAngles = new Vector3(356.2098F, 87.8449F, 299.6487F),
localScale = new Vector3(0.2F, 0.2F, 0.2F)
                }
});
            rules.Add("mdlLoader", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "MechHandR",
localPos = new Vector3(0.1491F, 0.0997F, -1.0425F),
localAngles = new Vector3(336.0122F, 34.7247F, 251.7394F),
localScale = new Vector3(0.06F, 0.06F, 0.06F)
                }
});
            rules.Add("mdlCroco", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Head",
localPos = new Vector3(0.0366F, 1.4401F, 1.3169F),
localAngles = new Vector3(87.3151F, 336.8591F, 333.4455F),
localScale = new Vector3(0.6F, 0.6F, 0.6F)
                }
});
            rules.Add("mdlCaptain", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
childName = "HandR",
localPos = new Vector3(-0.0732F, 0.0813F, 0.0077F),
localAngles = new Vector3(346.1867F, 14.9971F, 91.1527F),
localScale = new Vector3(0.03F, 0.03F, 0.03F)
                }
});
            return rules;
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
