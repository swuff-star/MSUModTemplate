using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using MonoMod.Cil;
using static LostInTransit.LostInTransitMain;

namespace LostInTransit.Items
{
    /*public class ImpOverlordsTentacle : ItemBase
    {
        public override string ItemName => "Imp Overlord's Tentacle";

        public override string ItemLangTokenName => "OverlordTentacle";

        public override string ItemPickupDesc => "Summon an Imp Bodyguard.";

        public override string ItemFullDescription => "Summon an Imp Bodyguard.";

        public override string ItemLore => "impy";

        public override ItemTier Tier => ItemTier.Tier2;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("Thallium.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("thallium.png");

        public static float impPer;

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateItem();
            Hooks();
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }

        public override void Hooks()
        {
            //On.RoR2.CharacterBody.FixedUpdate += SummonImp;
        }

        public void CreateConfig(ConfigFile config)
        {
            impPer = config.Bind<float>("Item: " + ItemName, "Cooldown", 1f, "Imps per tentacle.").Value;
        }

        private bool IsMinion(RoR2.CharacterMaster master)
        {
            return master.minionOwnership &&
                master.minionOwnership.ownerMaster;
        }

        //look at this later i dont think anyone actually wants this item

        private void UpdateBeetleGuardAllies()
        {
            if (NetworkServer.active)
            {
                int num = this.inventory ? this.inventory.GetItemCount(RoR2Content.Items.BeetleGland) : 0;
                if (num > 0)
                {
                    int deployableCount = this.master.GetDeployableCount(DeployableSlot.BeetleGuardAlly);
                    if (deployableCount < num)
                    {
                        this.guardResummonCooldown -= Time.fixedDeltaTime;
                        if (this.guardResummonCooldown <= 0f)
                        {
                            DirectorSpawnRequest directorSpawnRequest = new DirectorSpawnRequest((SpawnCard)Resources.Load("SpawnCards/CharacterSpawnCards/cscBeetleGuardAlly"), new DirectorPlacementRule
                            {
                                placementMode = DirectorPlacementRule.PlacementMode.Approximate,
                                minDistance = 3f,
                                maxDistance = 40f,
                                spawnOnTarget = this.transform
                            }, RoR2Application.rng);
                            directorSpawnRequest.summonerBodyObject = base.gameObject;
                            directorSpawnRequest.onSpawnedServer = new Action<SpawnCard.SpawnResult>(this.< UpdateBeetleGuardAllies > g__OnGuardMasterSpawned | 363_0);
                            DirectorCore.instance.TrySpawnObject(directorSpawnRequest);
                            if (deployableCount < num)
                            {
                                this.guardResummonCooldown = 1f;
                                return;
                            }
                            this.guardResummonCooldown = 30f;
                        }
                    }
                }
            }
        }
    }*/
}
