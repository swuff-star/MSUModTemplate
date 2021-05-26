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
using RoR2.Projectile;


namespace LostInTransit.Items
{
    public class TheAxe : ItemBase
    {
        public override string ItemName => "The Axe";

        public override string ItemLangTokenName => "THE_AXE";

        public override string ItemPickupDesc => "His licks were clean, but his riffs dirty...";

        public override string ItemFullDescription => "<style=cIsDamage>10%</style> chance to generate an <style=cIsDamage>explosive riff</style> on hit, dealing <style=cIsDamage>800%</style> <style=cStack>(+400% per stack)</style> damage and <style=cIsUtility>stunning</style> enemies within <style=cIsDamage>10m</style> <style=cStack>(+5m per stack)</style>.";

        public override string ItemLore => "\"So both guitars are playing the same riff, but one of them's playing it without the last note so it's in 13/8 instead of 7/4, right? And every time they play the riff the second guitar lags behind by an extra note, and they get more and more out of sync until eventually they're exactly one bar apart and they start playing at the same time again. It's some crazy shit bro, you gotta hear it.\"\n\n... \n\n\"Rad.\" \n\n... \n\n\"Can we kiss now?\"";

        public override ItemTier Tier => ItemTier.Tier3;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("Hit_List.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("HitList.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }

        public override void Hooks()
        {
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        

        public override void Init(ConfigFile config)
        {
            //CreateConfig(config);
            CreateLang();
            CreateItem();
            Hooks();
        }

        /*private static List<CharacterMaster> AliveList(bool playersOnly = false)
        {
            if (playersOnly) return PlayerCharacterMasterController.instances.Where(x => x.isConnected && x.master && x.master.hasBody && x.master.GetBody().healthComponent.alive).Select(x => x.master).ToList();
            else return CharacterMaster.readOnlyInstancesList.Where(x => x.hasBody && x.GetBody().healthComponent.alive).ToList();
        }*/
        
        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, RoR2.GlobalEventManager self, RoR2.DamageInfo damageInfo, GameObject victim)
        {
            var body = self.GetComponent<CharacterBody>();
            var axeCount = GetCount(body);
            orig(self, damageInfo, victim);
            if (axeCount > 0)
            {
                var attacker = body.gameObject;

                Vector3 corePos = attacker.transform.position;
                EffectManager.SpawnEffect(Resources.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXQuick"), new EffectData
                {
                    origin = corePos,
                    scale = 1,
                    rotation = Util.QuaternionSafeLookRotation(damageInfo.force)
                }, true);

                //fuck around with this later
                //stolen from behemoth code

                new BlastAttack
                {
                    attacker = attacker,
                    baseDamage = body.damage * 8f,
                    radius = 10f,
                    crit = damageInfo.crit,
                    falloffModel = BlastAttack.FalloffModel.None,
                    procCoefficient = 0f,
                    teamIndex = attacker.GetComponent<TeamIndex>(),
                    position = corePos,
                }.Fire();

                //apparently this code is somehow so abysmally bad it breaks all other on-hits
                //this is a proble mfor my sober self.
                //works now thanks komrade
            }

        }
    }
}
