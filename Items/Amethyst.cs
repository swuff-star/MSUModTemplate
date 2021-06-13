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

namespace LostInTransit.Equipment
{
    public class Amethyst : EquipmentBase

    {
        public override string EquipmentName => "Gigantic Amethyst";

        public override string EquipmentLangTokenName => "Gigantic_Amethyst";

        public override string EquipmentPickupDesc => "Reset cooldowns on use.";

        public override string EquipmentFullDescription => "Reset cooldowns on use.";

        public override string EquipmentLore => "[3:27 PM] the antichrist: i had a dream that i screamed the n word in front of a bunch of black people then i broke a cyanide capsule in my mouth and killed myself";

        public override GameObject EquipmentModel => MainAssets.LoadAsset<GameObject>("Amethyst.prefab");

        public static float amethystCooldown;

        public static GameObject ItemBodyModelPrefab;

        public override Sprite EquipmentIcon => MainAssets.LoadAsset<Sprite>("amethyst.png");
        public override bool AppearsInSinglePlayer { get; } = true;
        public override bool AppearsInMultiPlayer { get; } = true;
        public override bool CanDrop { get; } = true;
        
        public override float Cooldown => amethystCooldown;
        public override bool EnigmaCompatible { get; } = true;
        public override bool IsBoss { get; } = false;
        public override bool IsLunar { get; } = false;

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
            ItemBodyModelPrefab = EquipmentModel;
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
        }

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateEquipment();
            Hooks();
        }

        public void CreateConfig(ConfigFile config)
        {
            amethystCooldown = config.Bind<float>("Equipment: " + EquipmentName, "Cooldown", 18f, "Cooldown between uses").Value;
        }

        protected override bool ActivateEquipment(EquipmentSlot slot)
        {
            var sloc = slot.characterBody?.skillLocator;
            if (!sloc) return false;
            sloc.ApplyAmmoPack();
            return true;
        }
    }
}
