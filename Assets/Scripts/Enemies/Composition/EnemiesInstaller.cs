using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace AdventureIsland.Enemies
{
    [DisallowMultipleComponent]
    public sealed class EnemiesInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private VerticalSpiderEnemy verticalSpiderEnemyPrefab;
        [SerializeField] private StaticSpiderEnemy staticSpiderEnemyPrefab;
        [SerializeField] private BirdEnemy birdEnemyPrefab;

        public void Install(IContainerBuilder builder)
        {
            ValidateConfiguration();

            builder.Register<VerticalSpiderEnemyBuilder>(Lifetime.Scoped)
                .WithParameter(nameof(verticalSpiderEnemyPrefab), verticalSpiderEnemyPrefab)
                .As<IEnemyBuilder<VerticalSpiderEnemy>>();
            builder.Register<StaticSpiderEnemyBuilder>(Lifetime.Scoped)
                .WithParameter(nameof(staticSpiderEnemyPrefab), staticSpiderEnemyPrefab)
                .As<IEnemyBuilder<StaticSpiderEnemy>>();
            builder.Register<BirdEnemyBuilder>(Lifetime.Scoped)
                .WithParameter(nameof(birdEnemyPrefab), birdEnemyPrefab)
                .As<IEnemyBuilder<BirdEnemy>>();
            builder.Register(typeof(EnemyDirector<>), Lifetime.Scoped)
                .AsSelf();
            builder.Register(typeof(EnemyFactory<>), Lifetime.Scoped)
                .AsSelf();
        }

        private void ValidateConfiguration()
        {
            if (verticalSpiderEnemyPrefab == null)
            {
                throw new InvalidOperationException(
                    "EnemiesInstaller requires a Vertical Spider Enemy prefab reference.");
            }

            if (staticSpiderEnemyPrefab == null)
            {
                throw new InvalidOperationException(
                    "EnemiesInstaller requires a Static Spider Enemy prefab reference.");
            }

            if (birdEnemyPrefab == null)
            {
                throw new InvalidOperationException(
                    "EnemiesInstaller requires a Bird Enemy prefab reference.");
            }
        }
    }
}
