using UnityEngine;

namespace AdventureIsland.Combat
{
    public sealed class HammerProjectileBuilder
    {
        private HammerProjectile projectile;
        private Transform spawnPoint;

        public void Reset()
        {
            projectile = null;
            spawnPoint = null;
        }

        public void SetProduct(HammerProjectile product)
        {
            projectile = product;
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
