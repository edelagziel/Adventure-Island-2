using UnityEngine;

namespace AdventureIsland.Combat
{
    public interface IProjectileBuilder
    {
        void Reset();
        void SetProduct(Projectile product);
        void SetSpawnPoint(Transform spawnPoint);
        bool TryBuild();
    }
}
