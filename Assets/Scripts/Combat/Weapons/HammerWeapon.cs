using System;
using UnityEngine;

namespace AdventureIsland.Combat
{
    public sealed class HammerWeapon : IWeapon
    {
        private readonly ProjectileProvider<HammerProjectileDirector> projectileProvider;
        private readonly Transform spawnPoint;

        public int AvailableThrows { get; private set; }

        public HammerWeapon(
            ProjectileProvider<HammerProjectileDirector> projectileProvider,
            Transform spawnPoint)
        {
            this.projectileProvider = projectileProvider
                ?? throw new ArgumentNullException(nameof(projectileProvider));
            this.spawnPoint = spawnPoint;
        }

        public void CollectHammer()
        {
            AvailableThrows = checked(AvailableThrows + 1);
        }

        public bool TryAttack()
        {
            if (AvailableThrows == 0)
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

            AvailableThrows--;
            return true;
        }
    }
}
