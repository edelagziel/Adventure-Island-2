using System;
using UnityEngine;

namespace AdventureIsland.Combat
{
    public sealed class HammerWeapon : ICollectibleWeapon
    {
        private readonly ProjectileProvider<HammerProjectileDirector> projectileProvider;
        private readonly Transform spawnPoint;
        private Projectile activeProjectile;

        public int AvailableThrows { get; private set; }

        public HammerWeapon(
            ProjectileProvider<HammerProjectileDirector> projectileProvider,
            Transform spawnPoint)
        {
            this.projectileProvider = projectileProvider
                ?? throw new ArgumentNullException(nameof(projectileProvider));
            this.spawnPoint = spawnPoint;
        }

        public void Collect()
        {
            AvailableThrows = checked(AvailableThrows + 1);
        }

        public bool TryAttack()
        {
            if (AvailableThrows == 0 ||
                (activeProjectile != null && activeProjectile.gameObject.activeSelf))
            {
                return false;
            }

            Projectile projectile = projectileProvider.GetReady(spawnPoint);

            if (projectile == null)
            {
                return false;
            }

            if (!projectile.TryLaunch())
            {
                return false;
            }

            activeProjectile = projectile;
            AvailableThrows--;
            return true;
        }
    }
}
