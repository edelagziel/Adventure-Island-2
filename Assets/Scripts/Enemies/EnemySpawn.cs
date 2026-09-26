using System;
using UnityEngine;
using VContainer;

namespace AdventureIsland.Enemies
{
    [DisallowMultipleComponent]
    public sealed class EnemySpawn : MonoBehaviour, IStageResettable
    {
        [SerializeField] private EnemyDefinition definition;
        [SerializeField, Range(0f, 1f)] private float animalDropChance;
        [SerializeField] private GameObject animalDrop;

        private EnemyFactory enemyFactory;
        private Enemy spawnedEnemy;

        [Inject]
        public void Construct(EnemyFactory injectedEnemyFactory)
        {
            enemyFactory = injectedEnemyFactory
                ?? throw new ArgumentNullException(nameof(injectedEnemyFactory));
        }

        private void Start()
        {
            HideAnimalDrop();
            EnsureEnemyCreated();
        }

        public void ResetStageState()
        {
            HideAnimalDrop();

            if (spawnedEnemy == null)
            {
                EnsureEnemyCreated();
                return;
            }

            spawnedEnemy.ResetRuntimeState();
        }

        private void EnsureEnemyCreated()
        {
            if (spawnedEnemy != null)
            {
                return;
            }

            if (enemyFactory == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(EnemySpawn)} requires {nameof(EnemyFactory)} injection before creating an enemy.");
            }

            if (definition == null || definition.Prefab == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(EnemySpawn)} on '{name}' requires a valid EnemyDefinition.");
            }

            spawnedEnemy = enemyFactory.Create(
                definition,
                transform.position,
                transform.rotation,
                transform);

            if (spawnedEnemy == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(EnemyFactory)} failed to create the enemy for '{name}'.");
            }

            spawnedEnemy.DeathStarted += HandleEnemyDeathStarted;
        }

        private void OnDestroy()
        {
            if (spawnedEnemy != null)
            {
                spawnedEnemy.DeathStarted -= HandleEnemyDeathStarted;
            }
        }

        private void HandleEnemyDeathStarted(Enemy enemy)
        {
            if (animalDrop == null || animalDrop.activeSelf ||
                animalDropChance <= 0f || UnityEngine.Random.value > animalDropChance)
            {
                return;
            }

            animalDrop.transform.position = enemy.transform.position;
            animalDrop.SetActive(true);
        }

        private void HideAnimalDrop()
        {
            if (animalDrop != null)
            {
                animalDrop.SetActive(false);
            }
        }
    }
}
