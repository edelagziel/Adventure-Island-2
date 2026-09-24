using System;
using UnityEngine;

namespace AdventureIsland.Combat
{
    public sealed class BoomerangWeapon : ICollectibleWeapon
    {
        private readonly ProjectileProvider<BoomerangProjectileDirector> projectileProvider;
        private readonly Transform spawnPoint;
        private Projectile activeProjectile;
        private bool collected;

        public BoomerangWeapon(
            ProjectileProvider<BoomerangProjectileDirector> projectileProvider,
            Transform spawnPoint)
        {
            this.projectileProvider = projectileProvider
                ?? throw new ArgumentNullException(nameof(projectileProvider));
            this.spawnPoint = spawnPoint;
        }

        public void Collect()
        {
            collected = true;
        }

        public bool TryAttack()
        {
            if (!collected || (activeProjectile != null && activeProjectile.gameObject.activeSelf))
            {
                return false;
            }

            Projectile projectile = projectileProvider.GetReady(spawnPoint);
            if (projectile == null || !projectile.TryLaunch())
            {
                return false;
            }

            activeProjectile = projectile;
            return true;
        }
    }
}
