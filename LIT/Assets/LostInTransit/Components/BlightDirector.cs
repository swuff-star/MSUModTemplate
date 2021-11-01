using LostInTransit.Equipments;
using Moonstorm;
using RoR2;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace LostInTransit.Components
{
    public class BlightDirector : NetworkBehaviour
    {
        public static BlightDirector Instance { get; private set; }
        public const float maxSpawnRate = 1f;
        public const float minTimeBeforeKillsCount = 1200f; //This roughly causes kills to only count past 20 minutes in drizzle.
        public const float spawnRatePerMonsterKilled = 0.001f;
        private PlayerCharacterMasterController[] PlayerCharMasters { get => PlayerCharacterMasterController.instances.ToArray(); }
        public DifficultyDef RunDifficulty { get => DifficultyCatalog.GetDifficultyDef(Run.instance.selectedDifficulty); }
        public float MaxSpawnRateWithDiffCoef { get => (maxSpawnRate * RunDifficulty.scalingValue) + GetTotalBeadCount(); }
        public float MinTimeBeforeKillsCountWithDiffCoef { get => (minTimeBeforeKillsCount / RunDifficulty.scalingValue); }
        public int TelebossCostMultiplier { get => LITConfig.TPBlightCost.Value; }
        private SceneDef CommencementScene { get => SceneCatalog.GetSceneDefFromSceneName("moon2"); }
        private EquipmentIndex BlightedEquipIndex { get => Assets.LITAssets.LoadAsset<EquipmentDef>("AffixBlighted").equipmentIndex; }
        private bool IsPrestigeArtifactEnabled
        {
            get
            {
                var blightArtifact = Assets.LITAssets.LoadAsset<ArtifactDef>("Prestige");
                if (blightArtifact)
                {
                    return RunArtifactManager.instance.IsArtifactEnabled(blightArtifact);
                }
                return false;
            }
        }
        [SyncVar]
        private float SpawnRate = 0;
        [SyncVar]
        private ulong monstersKilled;

        void Start()
        {
            Instance = this;
            SpawnRate = 0;
            monstersKilled = 0;
            RecalculateSpawnChance();

            GlobalEventManager.onCharacterDeathGlobal += OnEnemyKilled;
            CharacterBody.onBodyStartGlobal += TrySpawn;
        }

        [Server]
        private void OnEnemyKilled(DamageReport obj)
        {
            if (Run.instance.GetRunStopwatch() > MinTimeBeforeKillsCountWithDiffCoef)
            {
                var victimBody = obj.victimBody;
                var attackerBody = obj.attackerBody;
                if (victimBody && attackerBody)
                {
                    var victimTeamComponent = victimBody.teamComponent;
                    if (victimTeamComponent)
                    {
                        if (victimTeamComponent.teamIndex == TeamIndex.Monster || victimTeamComponent.teamIndex == TeamIndex.Lunar)
                        {
                            if (attackerBody.isPlayerControlled && SpawnRate < MaxSpawnRateWithDiffCoef)
                            {
                                monstersKilled += 1 * ((ulong)Run.instance?.loopClearCount + 1);
                                RecalculateSpawnChance();
                            }
                        }
                    }
                }
            }
        }

        private void TrySpawn(CharacterBody body)
        {
            bool stageNotCommencement = (Stage.instance?.sceneDef?.sceneDefIndex != CommencementScene.sceneDefIndex);
            bool isEnemy = (body.teamComponent?.teamIndex != TeamIndex.Player);
            bool hasComponent = ((bool)body.master?.GetComponent<BlightedController>());
            int cost = GetCost(body);

            if (cost < 0)
                return;

            if (stageNotCommencement && isEnemy && hasComponent)
            {
                if (CheckIfCostIsSufficient(cost) && Util.CheckRoll(SpawnRate))
                {
                    monstersKilled -= (ulong)cost;
                    MakeBlighted(body);
                }
            }
        }

        private void MakeBlighted(CharacterBody body)
        {
            var inventory = body.inventory;

            inventory.SetEquipmentIndex(BlightedEquipIndex);
            body.isElite = true;

            inventory.RemoveItem(RoR2Content.Items.BoostHp, inventory.GetItemCount(RoR2Content.Items.BoostHp));
            inventory.RemoveItem(RoR2Content.Items.BoostDamage, inventory.GetItemCount(RoR2Content.Items.BoostDamage));

            DeathRewards rewards = body.GetComponent<DeathRewards>();
            if (rewards)
            {
                rewards.expReward *= 5;
                rewards.goldReward *= 5;
            }
        }

        private int GetCost(CharacterBody body)
        {
            if(Elites.Blight.blightCostdictionary.TryGetValue(body.bodyIndex, out int value))
            {
                if(body.isBoss)
                {
                    return value * TelebossCostMultiplier;
                }
                return value;
            }
            else
            {
                return -1;
            }
        }

        private bool CheckIfCostIsSufficient(int cost)
        {
            return (monstersKilled > (ulong)cost);
        }

        [Server]
        private void RecalculateSpawnChance()
        {
            if (IsPrestigeArtifactEnabled)
            {
                SpawnRate = 10f;
                return;
            }

            float baseSpawnChance = 0;
            if (RunDifficulty.scalingValue > 3)
                baseSpawnChance = 0.1f * (RunDifficulty.scalingValue - 2);

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

        void OnDestroy()
        {
            GlobalEventManager.onCharacterDeathGlobal -= OnEnemyKilled;
            CharacterBody.onBodyStartGlobal -= TrySpawn;
        }
    }
}