using System;
using UnityEngine;
using VContainer;

namespace AdventureIsland.Enemies
{
    [DisallowMultipleComponent]
    public sealed class EnemySpawn : MonoBehaviour, IStageResettable
    {
        [SerializeField] private EnemyDefinition definition;

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
            EnsureEnemyCreated();
        }

        public void ResetStageState()
        {
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
        }
    }
}
