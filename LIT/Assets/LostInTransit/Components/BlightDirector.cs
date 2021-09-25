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

        public const float maxSpawnRate = 1.5f;

        public float MaxSpawnRateWithDiffCoef { get => maxSpawnRate * RunDifficulty.scalingValue; }

        [SyncVar]
        public float SpawnRate = 0;

        public const float spawnRatePerMonsterKilled = 0.0025f;

        [SyncVar]
        private ulong monstersKilled;

        private bool IsArtifactEnabled
        {
            get
            {
                var blightArtifact = Assets.LITAssets.LoadAsset<ArtifactDef>("Blighted");
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
            LITLogger.LogI($"BlightDirector.Start()");
            Instance = this;
            DontDestroyOnLoad(gameObject);
            RecalculateSpawnChance();

            GlobalEventManager.onCharacterDeathGlobal += OnEnemyKilled;
            CharacterBody.onBodyStartGlobal += MakeBlighted;
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
            if(obj.victimTeamIndex == TeamIndex.Monster || obj.victimTeamIndex == TeamIndex.Lunar)
            {
                if(obj.attackerBody.isPlayerControlled)
                {
                    monstersKilled++;
                    RecalculateSpawnChance();
                }
            }
        }

        [Server]
        private void MakeBlighted(CharacterBody body)
        {
            LITLogger.LogI($"Attempting to turn {body} into a blighted elite.");
            if (Stage.instance?.sceneDef?.sceneDefIndex != CommencementScene.sceneDefIndex)
                if (body.teamComponent?.teamIndex != TeamIndex.Player)
                    if (body.master?.GetComponent<BlightedController>() && Util.CheckRoll(SpawnRate))
                    {
                        LITLogger.LogE($"Turning {body} into a blighted enemy.");
                        var inventory = body.inventory;
                        
                        inventory.SetEquipmentIndex(BlightedEquipIndex);
                        body.isElite = true;

                        inventory.RemoveItem(RoR2Content.Items.BoostHp, inventory.GetItemCount(RoR2Content.Items.BoostHp));
                        inventory.RemoveItem(RoR2Content.Items.BoostDamage, inventory.GetItemCount(RoR2Content.Items.BoostDamage));
                    }
                        
        }

        [Server]
        private void RecalculateSpawnChance()
        {
            LITLogger.LogI($"Old spawn rate: {SpawnRate}");
            if(IsArtifactEnabled)
            {
                SpawnRate = 10f;
                return;
            }

            float baseSpawnChance = 0;
            if (RunDifficulty.scalingValue > 3)
                baseSpawnChance = 0.5f * (RunDifficulty.scalingValue / 3);

            float monstersKilledModifier = (monstersKilled * spawnRatePerMonsterKilled) * RunDifficulty.scalingValue;

            float beadsModifier;
            int sharedBeadCount = 0;
            for(int i = 0; i < PlayerCharMasters.Length; i++)
            {
                var inventory = PlayerCharMasters[i].master?.inventory;
                sharedBeadCount += inventory.GetItemCount(RoR2Content.Items.LunarTrinket);
            }
            beadsModifier = sharedBeadCount * 0.5f;


            float finalSpawnChance = baseSpawnChance + monstersKilledModifier + beadsModifier;
            SpawnRate = Mathf.Min(finalSpawnChance, MaxSpawnRateWithDiffCoef);
            LITLogger.LogI($"New spawn rate: {SpawnRate}");
        }
    }
}