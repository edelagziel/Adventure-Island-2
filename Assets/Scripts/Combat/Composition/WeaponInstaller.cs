using UnityEngine;
using System;
using VContainer;
using VContainer.Unity;

namespace AdventureIsland.Combat
{
    [DisallowMultipleComponent]
    public sealed class WeaponInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private Projectile hammerProjectilePrefab;
        [SerializeField] private Transform hammerSpawnPoint;
        [SerializeField] private Projectile boomerangProjectilePrefab;
        [SerializeField] private Transform boomerangSpawnPoint;

        private const string BoomerangPoolKey = "BoomerangProjectilePool";

        public void Install(IContainerBuilder builder)
        {
            if (hammerProjectilePrefab == null || hammerSpawnPoint == null)
            {
                throw new InvalidOperationException(
                    "WeaponInstaller requires a Hammer projectile prefab and spawn point.");
            }

            if (boomerangProjectilePrefab == null || boomerangSpawnPoint == null)
            {
                throw new InvalidOperationException(
                    "WeaponInstaller requires a Boomerang projectile prefab and spawn point.");
            }

            builder.Register<WeaponController>(Lifetime.Scoped)
                .AsSelf();
            builder.Register<PlayerWeaponCollector>(Lifetime.Scoped)
                .AsSelf();
            builder.Register<PlayerAttackController>(Lifetime.Scoped)
                .AsSelf();
            builder.RegisterBuildCallback(container =>
                container.Resolve<PlayerAttackController>()
                    .SetDefaultAttackSource(container.Resolve<WeaponController>()));
            builder.Register<HammerWeapon>(Lifetime.Scoped)
                .WithParameter("spawnPoint", hammerSpawnPoint)
                .AsSelf();
            builder.Register<HammerProjectileBuilder>(Lifetime.Transient)
                .AsSelf();
            builder.Register<HammerProjectileDirector>(Lifetime.Scoped)
                .WithParameter("builder", resolver => resolver.Resolve<HammerProjectileBuilder>())
                .AsSelf();
            builder.Register<ProjectileFactory>(Lifetime.Scoped)
                .AsSelf();
            builder.Register<ProjectilePool>(Lifetime.Scoped)
                .WithParameter("prefab", hammerProjectilePrefab)
                .AsSelf();
            builder.Register<ProjectileProvider<HammerProjectileDirector>>(Lifetime.Scoped)
                .AsSelf();
            builder.Register<BoomerangWeapon>(Lifetime.Scoped)
                .WithParameter("spawnPoint", boomerangSpawnPoint)
                .AsSelf();
            builder.Register<BoomerangProjectileBuilder>(Lifetime.Transient)
                .AsSelf();
            builder.Register<BoomerangProjectileDirector>(Lifetime.Scoped)
                .WithParameter("builder", resolver => resolver.Resolve<BoomerangProjectileBuilder>())
                .AsSelf();
            builder.Register<ProjectilePool>(Lifetime.Scoped)
                .Keyed(BoomerangPoolKey)
                .WithParameter("prefab", boomerangProjectilePrefab)
                .AsSelf();
            builder.Register<ProjectileProvider<BoomerangProjectileDirector>>(Lifetime.Scoped)
                .WithParameter("pool", resolver => resolver.Resolve<ProjectilePool>(BoomerangPoolKey))
                .AsSelf();
        }
    }
}
