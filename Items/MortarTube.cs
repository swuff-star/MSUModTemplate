using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static LostInTransit.LostInTransitMain;

namespace LostInTransit.Items
{
    public class MortarTube : ItemBase
    {
        public override string ItemName => "Mortar Tube";

        public override string ItemLangTokenName => "MORTAR";

        public override string ItemPickupDesc => "Chance to fire a mortar on-hit.";

        public override string ItemFullDescription => $"<style=cIsDamage>{MortarTube.procChance}%</style> chance to launch a <style=cIsDamage>mortar</style> <style=cStack>(+{MortarTube.stackAmount} per stack)</style> that deals <style=cIsDamage>{MortarTube.dmgCoefficient * 100}%</style> TOTAL damage in an area of effect.";

        public override string ItemLore => "vwomp psssssssshhhhhhhhhhhhhhhhhhheww!!";

        public override ItemTier Tier => ItemTier.Tier1;

        public static GameObject ItemBodyModelPrefab;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("Mortar.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("mortar.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemBodyModelPrefab = ItemModel;
            var itemDisplay = ItemBodyModelPrefab.AddComponent<ItemDisplay>();
            itemDisplay.rendererInfos = ItemDisplaySetup(ItemBodyModelPrefab, true);

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
localPos = new Vector3(-0.10768F, 0.3627F, -0.16637F),
localAngles = new Vector3(2.72863F, 85.30157F, 321.4987F),
localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });
            rules.Add("mdlHuntress", new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                   childName = "Chest",
localPos = new Vector3(0.18296F, 0.07468F, -0.07853F),
localAngles = new Vector3(19.39358F, 138.7414F, 315.3141F),
localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });
            rules.Add("mdlBandit2", new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Chest",
localPos = new Vector3(-0.05786F, 0.27645F, -0.12947F),
localAngles = new Vector3(352.4726F, 20.62009F, 8.99585F),
localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });
            rules.Add("mdlToolbot", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
childName = "Head",
localPos = new Vector3(1.06204F, 1.29983F, 0.92494F),
localAngles = new Vector3(356.2359F, 269.9657F, 303.9216F),
localScale = new Vector3(0.7F, 0.7F, 0.7F)
                }
});
            rules.Add("mdlEngi", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
childName = "CannonHeadL",
localPos = new Vector3(-0.02132F, 0.31746F, 0.18864F),
localAngles = new Vector3(354.3041F, 270.9333F, 268.3198F),
localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
});
            rules.Add("mdlMage", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
childName = "Chest",
localPos = new Vector3(0.24264F, -0.03328F, -0.40011F),
localAngles = new Vector3(356.408F, 241.8843F, 308.2445F),
localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
});
            rules.Add("mdlMerc", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
childName = "Chest",
localPos = new Vector3(0.20419F, 0.0824F, -0.29713F),
localAngles = new Vector3(357.3375F, 214.6085F, 309.91F),
localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
});
            rules.Add("mdlTreebot", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
childName = "WeaponPlatformEnd",
localPos = new Vector3(-0.02056F, -0.95953F, 0.37187F),
localAngles = new Vector3(357.0735F, 270.3329F, 316.1714F),
localScale = new Vector3(0.2F, 0.2F, 0.2F)
                }
});
            rules.Add("mdlLoader", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Chest",
localPos = new Vector3(-0.16877F, 0.48488F, -0.26781F),
localAngles = new Vector3(350.37F, 71.98739F, 352.4411F),
localScale = new Vector3(0.07F, 0.07F, 0.07F)
                }
});
            rules.Add("mdlCroco", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Head",
localPos = new Vector3(-0.23833F, -0.02393F, 2.60071F),
localAngles = new Vector3(2.03791F, 91.20681F, 324.0835F),
localScale = new Vector3(1F, 1F, 1F)
                }
});
            rules.Add("mdlCaptain", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "HandL",
localPos = new Vector3(-0.11004F, 0.14844F, -0.06651F),
localAngles = new Vector3(1.17231F, 278.3796F, 313.2302F),
localScale = new Vector3(0.08F, 0.08F, 0.08F)
                }
});
            return rules;
        }

        public static float procChance;
        public static float stackChance;
        public static float capChance;
        public static float dmgCoefficient;
        public static float dmgStack;
        public static float velocityMultiplier;
        public static float gravityAmount;
        public static bool fixedAim;
        public static float launchAngle;
        public static float inaccuracyRate;
        public static float stackAmount;

        public void CreateConfig(ConfigFile config)
        {
            procChance = config.Bind<float>("Item: " + ItemName, "Base Proc Chance", 9f, "Base chance of proc'ing.").Value;
            stackChance = config.Bind<float>("Item: " + ItemName, "Stacking Proc Chance", 0f, "Added chance to proc per stack.").Value;
            capChance = config.Bind<float>("Item: " + ItemName, "Maximum Proc Chance", 100f, "Maximum chance of proc'ing.").Value;
            dmgCoefficient = config.Bind<float>("Item: " + ItemName, "Base Damage", 1.7f, "Base damage.").Value;
            dmgStack = config.Bind<float>("Item: " + ItemName, "Stacking Damage", 0f, "Added damage per stack.").Value;
            velocityMultiplier = config.Bind<float>("Item: " + ItemName, "Velocity", 0.5f, "Velocity of the mortar - higher equates to faster.").Value;
            gravityAmount = config.Bind<float>("Item: " + ItemName, "Gravity", 0.5f, "Weight of the mortar - higher equates to heavier.").Value;
            fixedAim = config.Bind<bool>("Item: " + ItemName, "Fixed Aim", false, "Fire mortar at a fixed angle every time.").Value;
            launchAngle = config.Bind<float>("Item: " + ItemName, "Launch Angle", 0.8f, "The angle the mortar is launched - 1 being straight up, -1 being straight down.").Value;
            inaccuracyRate = config.Bind<float>("Item: " + ItemName, "Inaccuracy Rate", 0.25f, "Inaccuracy of the mortar - higher equates to less accuracy.").Value;
            stackAmount = config.Bind<float>("Item: " + ItemName, "Stacking Projectile Count", 1f, "Amount of mortars launched per stack.").Value;
        }

        public static CharacterModel.RendererInfo[] ItemDisplaySetup(GameObject obj, bool debugmode = false)
        {
            MeshRenderer[] meshes = obj.GetComponentsInChildren<MeshRenderer>();
            CharacterModel.RendererInfo[] renderInfos = new CharacterModel.RendererInfo[meshes.Length];

            for (int i = 0; i < meshes.Length; i++)
            {
                if (debugmode)
                {
                    var controller = meshes[i].gameObject.AddComponent<MaterialControllerComponents.HGControllerFinder>();
                    controller.MeshRenderer = meshes[i];
                }
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

        public static GameObject mortarPrefab { get; private set; }

        private void On_GEMOnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig(self, damageInfo, victim);

            if (!NetworkServer.active || !victim || !damageInfo.attacker || damageInfo.procCoefficient <= 0f || damageInfo.procChainMask.HasProc(ProcType.Missile)) return;

            var vicb = victim.GetComponent<CharacterBody>();

            CharacterBody body = damageInfo.attacker.GetComponent<CharacterBody>();
            if (!body || !vicb || !vicb.healthComponent || !vicb.mainHurtBox) return;

            CharacterMaster chrm = body.master;
            if (!chrm) return;

            int icnt = GetCount(chrm);
            if (icnt == 0) return;

            float m2Proc = procChance;
            if (icnt > 0) m2Proc += stackChance * icnt;
            if (m2Proc > capChance) m2Proc = capChance;
            if (!Util.CheckRoll(m2Proc * damageInfo.procCoefficient, chrm)) return;
            LaunchMortar(body, damageInfo.procChainMask, victim, damageInfo, icnt);
        }

        private void LaunchMortar(CharacterBody attackerBody, ProcChainMask procChainMask, GameObject victim, DamageInfo damageInfo, int stack)
        {
            GameObject gameObject = attackerBody.gameObject;
            InputBankTest component = gameObject.GetComponent<InputBankTest>();
            Vector3 position = component ? component.aimOrigin : gameObject.transform.position;

            float dmgCoef = dmgCoefficient + (dmgStack * stack);
            float damage = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, dmgCoef);
            ProcChainMask procChainMask2 = procChainMask;
            procChainMask2.AddProc(ProcType.Missile);
            FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
            {
                projectilePrefab = mortarPrefab,
                position = position,
                procChainMask = procChainMask2,
                target = victim,
                owner = gameObject,
                damage = damage,
                crit = damageInfo.crit,
                force = 500f,
                damageColorIndex = DamageColorIndex.Item,
                speedOverride = -1f,
                damageTypeOverride = DamageType.AOE
            };
            int times = (int)(1 * stack);
            for (int t = 0; t < times; t++)
            {
                Vector3 direction;
                if (fixedAim) direction = gameObject.transform.forward;
                else direction = component ? component.aimDirection : gameObject.transform.forward;
                direction = direction.normalized + new Vector3(0f, launchAngle, 0f);
                direction += new Vector3(UnityEngine.Random.Range(-inaccuracyRate, inaccuracyRate),
                    UnityEngine.Random.Range(-inaccuracyRate, inaccuracyRate),
                    UnityEngine.Random.Range(inaccuracyRate, inaccuracyRate));
                fireProjectileInfo.rotation = Util.QuaternionSafeLookRotation(direction);
                ProjectileManager.instance.FireProjectile(fireProjectileInfo);
            }
        }
        public static void SetupMortarProjectile()
        {
            GameObject paladinRocket = Resources.Load<GameObject>("prefabs/projectiles/PaladinRocket");
            mortarPrefab = paladinRocket.InstantiateClone("MortarProjectile");
            mortarPrefab.AddComponent<MortarGravity>();
            var model = MainAssets.LoadAsset<GameObject>("MortarMissile.prefab");
            model.AddComponent<NetworkIdentity>();
            model.AddComponent<RoR2.Projectile.ProjectileGhostController>();
            var controller = mortarPrefab.GetComponent < RoR2.Projectile.ProjectileController>();
            controller.ghostPrefab = model;
            ProjectileAPI.Add(mortarPrefab);
        }

        internal class MortarGravity : MonoBehaviour
        {
            private ProjectileSimple projSimp;

            private void Awake()
            {
                projSimp = gameObject.GetComponent<ProjectileSimple>();
                if (!projSimp) return;
                projSimp.desiredForwardSpeed *= velocityMultiplier;
            }

            private void FixedUpdate()
            {
                if (!projSimp) return;
                projSimp.rigidbody.velocity -= new Vector3(0, gravityAmount, 0);
            }
        }

        public override void Hooks()
        {
            On.RoR2.GlobalEventManager.OnHitEnemy += On_GEMOnHitEnemy;
        }

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateItem();
            SetupMortarProjectile();
            Hooks();
        }
    }
}
