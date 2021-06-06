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

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("Hit_List.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("HitList.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
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
            if (mortarPrefab) return;
            GameObject paladinRocket = Resources.Load<GameObject>("prefabs/projectiles/PaladinRocket");
            mortarPrefab = paladinRocket.InstantiateClone("MortarProjectile");
            mortarPrefab.AddComponent<MortarGravity>();
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
