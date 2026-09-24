using UnityEngine;

namespace AdventureIsland.Combat
{
    public sealed class ProjectileFactory
    {
        public Projectile Create(Projectile prefab)
        {
            return prefab != null ? Object.Instantiate(prefab) : null;
        }
    }
}
