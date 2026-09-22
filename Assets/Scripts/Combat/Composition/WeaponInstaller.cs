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

        public void Install(IContainerBuilder builder)
        {
            if (hammerProjectilePrefab == null || hammerSpawnPoint == null)
            {
                throw new InvalidOperationException(
                    "WeaponInstaller requires a Hammer projectile prefab and spawn point.");
            }

            builder.Register<WeaponController>(Lifetime.Scoped)
                .AsSelf();
            builder.Register<PlayerWeaponCollector>(Lifetime.Scoped)
                .AsSelf();
            builder.Register<PlayerAttackController>(Lifetime.Scoped)
                .AsSelf();
            builder.Register<HammerWeapon>(Lifetime.Scoped)
                .WithParameter("spawnPoint", hammerSpawnPoint)
                .AsSelf();
            builder.Register<HammerProjectileBuilder>(Lifetime.Transient)
                .AsSelf();
            builder.Register<HammerProjectileDirector>(Lifetime.Scoped)
                .AsSelf();
            builder.Register<ProjectileFactory>(Lifetime.Scoped)
                .AsSelf();
            builder.Register<ProjectilePool>(Lifetime.Scoped)
                .WithParameter("prefab", hammerProjectilePrefab)
                .AsSelf();
            builder.Register<ProjectileProvider<HammerProjectileDirector>>(Lifetime.Scoped)
                .AsSelf();
        }
    }
}
