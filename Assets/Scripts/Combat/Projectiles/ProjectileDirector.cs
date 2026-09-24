using UnityEngine;

namespace AdventureIsland.Combat
{
    public abstract class ProjectileDirector
    {
        public abstract bool TryConstruct(Projectile projectile, Transform spawnPoint);
    }
}
