using UnityEngine;

namespace AdventureIsland.Combat
{
    public sealed class HammerProjectileBuilder : IProjectileBuilder
    {
        private HammerProjectile projectile;
        private Transform spawnPoint;

        public void Reset()
        {
            projectile = null;
            spawnPoint = null;
        }

        public void SetProduct(Projectile product)
        {
            projectile = product as HammerProjectile;
        }

        public void SetSpawnPoint(Transform point)
        {
            spawnPoint = point;
        }

        public bool TryBuild()
        {
            bool configured = projectile != null &&
                projectile.TryConfigure(spawnPoint);
            Reset();
            return configured;
        }
    }
}
