using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using AdventureIsland.Combat;

namespace AdventureIsland.Enemies
{
    [DisallowMultipleComponent]
    public sealed class EnemiesInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private FireSnakeProjectile fireSnakeProjectilePrefab;
        [SerializeField] private Transform fireSnakeProjectilePoolRoot;

        private const string FireSnakeProjectilePoolKey = "FireSnakeProjectilePool";

        public void Install(IContainerBuilder builder)
        {
            builder.Register<EnemyBuilder>(Lifetime.Scoped)
                .As<IEnemyBuilder>();
            builder.Register<EnemyDirector>(Lifetime.Scoped)
                .AsSelf();
            builder.Register<EnemyFactory>(Lifetime.Scoped)
                .AsSelf();

            RegisterPlayerTransformProvider(builder);
            RegisterFireSnakeProjectilePool(builder);
        }

        private void RegisterPlayerTransformProvider(IContainerBuilder builder)
        {
            if (playerTransform == null)
            {
                return;
            }

            builder.RegisterInstance<IPlayerTransformProvider>(
                new PlayerTransformProvider(playerTransform));
        }

        private void RegisterFireSnakeProjectilePool(IContainerBuilder builder)
        {
            if (fireSnakeProjectilePrefab == null &&
                fireSnakeProjectilePoolRoot == null)
            {
                return;
            }

            if (fireSnakeProjectilePrefab == null ||
                fireSnakeProjectilePoolRoot == null)
            {
                throw new InvalidOperationException(
                    "EnemiesInstaller requires both a Fire Snake projectile prefab " +
                    "and projectile pool root when Fire Snake pooling is configured.");
            }

            builder.Register<ProjectilePool>(Lifetime.Scoped)
                .Keyed(FireSnakeProjectilePoolKey)
                .WithParameter("prefab", (Projectile)fireSnakeProjectilePrefab)
                .WithParameter("projectileRoot", fireSnakeProjectilePoolRoot)
                .AsSelf();
            builder.Register<FireSnakeProjectileProvider>(Lifetime.Scoped)
                .WithParameter(
                    "pool",
                    resolver => resolver.Resolve<ProjectilePool>(FireSnakeProjectilePoolKey))
                .AsSelf();
        }
    }
}
