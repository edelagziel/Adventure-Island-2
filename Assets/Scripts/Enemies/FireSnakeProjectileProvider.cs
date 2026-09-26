using System;
using AdventureIsland.Combat;
using UnityEngine;

namespace AdventureIsland.Enemies
{
    public sealed class FireSnakeProjectileProvider
    {
        private readonly ProjectilePool pool;

        public FireSnakeProjectileProvider(ProjectilePool pool)
        {
            this.pool = pool ?? throw new ArgumentNullException(nameof(pool));
        }

        public FireSnakeProjectile Get(Vector3 position, Quaternion rotation)
        {
            Projectile pooledProjectile = pool.Acquire();
            if (pooledProjectile is not FireSnakeProjectile projectile)
            {
                if (pooledProjectile != null)
                {
                    pool.Release(pooledProjectile);
                }

                return null;
            }

            projectile.transform.SetPositionAndRotation(position, rotation);
            return projectile;
        }
    }
}
