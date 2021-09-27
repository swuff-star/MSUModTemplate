using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using Moonstorm;
using System;
using LostInTransit.Modules;
using System.Linq;
using UnityEngine.SceneManagement;
using LostInTransit.Utils;

namespace LostInTransit.Components
{
    public class BlightDirector : NetworkBehaviour
    {
        public static BlightDirector Instance { get; private set; }

        private PlayerCharacterMasterController[] PlayerCharMasters { get => PlayerCharacterMasterController.instances.ToArray(); }

        public DifficultyDef RunDifficulty { get => DifficultyCatalog.GetDifficultyDef(Run.instance.selectedDifficulty); }

        public const float maxSpawnRate = 1f;

        public float MaxSpawnRateWithDiffCoef { get => maxSpawnRate * RunDifficulty.scalingValue * GetTotalBeadCount(); }

        [SyncVar]
        public float SpawnRate = 0;

        public const float spawnRatePerMonsterKilled = 0.001f;

        [SyncVar]
        private ulong monstersKilled;

        private bool IsArtifactEnabled
        {
            get
            {
                var blightArtifact = Assets.LITAssets.LoadAsset<ArtifactDef>("Prestige");
                if(blightArtifact)
                {
                    return RunArtifactManager.instance.IsArtifactEnabled(blightArtifact);
                }
                return false;
            }
        }

        private SceneDef CommencementScene { get => SceneCatalog.GetSceneDefFromSceneName("moon2"); }

        private EquipmentIndex BlightedEquipIndex { get => Assets.LITAssets.LoadAsset<EquipmentDef>("AffixBlighted").equipmentIndex; }
        
        void Start()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            RecalculateSpawnChance();

            GlobalEventManager.onCharacterDeathGlobal += OnEnemyKilled;
            CharacterBody.onBodyStartGlobal += TrySpawn;
            Run.onRunDestroyGlobal += Reset;
        }

        private void Reset(Run obj)
        {
            monstersKilled = 0;
            SpawnRate = 0;
        }

        [Server]
        private void OnEnemyKilled(DamageReport obj)
        {
            if (obj.victimTeamIndex == TeamIndex.Monster || obj.victimTeamIndex == TeamIndex.Lunar)
            {
                if((bool)obj.attackerBody?.isPlayerControlled)
                {
                    monstersKilled++;
                    RecalculateSpawnChance();
                }
            }
        }

        private void TrySpawn(CharacterBody body)
        {
            var flag1 = (Stage.instance?.sceneDef?.sceneDefIndex != CommencementScene.sceneDefIndex);
            var flag2 = (body.teamComponent?.teamIndex != TeamIndex.Player);
            var flag3 = ((bool)body.master?.GetComponent<BlightedController>());
            var flag4 = !body.isChampion;
            if(flag1 && flag2 && flag3 && flag4)
            {
                if(body.isBoss)
                    if(Util.CheckRoll(SpawnRate, -1))
                        MakeBlighted(body);
                else if(Util.CheckRoll(SpawnRate))
                    MakeBlighted(body);
            }       
        }

        private void MakeBlighted(CharacterBody body)
        {
            if (body.master?.GetComponent<BlightedController>() && Util.CheckRoll(SpawnRate))
            {
                var inventory = body.inventory;

                inventory.SetEquipmentIndex(BlightedEquipIndex);
                body.isElite = true;

                inventory.RemoveItem(RoR2Content.Items.BoostHp, inventory.GetItemCount(RoR2Content.Items.BoostHp));
                inventory.RemoveItem(RoR2Content.Items.BoostDamage, inventory.GetItemCount(RoR2Content.Items.BoostDamage));

                DeathRewards rewards = body.GetComponent<DeathRewards>();
                if (rewards)
                {
                    rewards.expReward *= 2;
                    rewards.goldReward *= 2;
                }
            }
        }

        [Server]
        private void RecalculateSpawnChance()
        {
            if (!NetworkServer.active)
                return;
            if(IsArtifactEnabled)
            {
                SpawnRate = 10f;
                return;
            }

            float baseSpawnChance = 0;
            if (RunDifficulty.scalingValue > 3)
                baseSpawnChance = 0.1f * (RunDifficulty.scalingValue / 3);

            float monstersKilledModifier = 0;
            int divisor = 1;
            if (RunArtifactManager.instance.IsArtifactEnabled(RoR2Content.Artifacts.Swarms))
                divisor = 2;
            monstersKilledModifier = (monstersKilled * spawnRatePerMonsterKilled * RunDifficulty.scalingValue) / divisor;

            float finalSpawnChance = baseSpawnChance + monstersKilledModifier;
            SpawnRate = Mathf.Min(finalSpawnChance, MaxSpawnRateWithDiffCoef);
        }

        private float GetTotalBeadCount()
        {
            float sharedBeadCount = 0;
            for (int i = 0; i < PlayerCharMasters.Length; i++)
            {
                var inventory = PlayerCharMasters[i].master?.inventory;
                sharedBeadCount += inventory.GetItemCount(RoR2Content.Items.LunarTrinket);
            }
            return sharedBeadCount;
        }
    }
}