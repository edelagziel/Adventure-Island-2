using System;
using UnityEngine;

namespace AdventureIsland.Combat
{
    public sealed class ProjectileProvider<TDirector>
        where TDirector : ProjectileDirector
    {
        private readonly ProjectilePool pool;
        private readonly TDirector director;

        public ProjectileProvider(ProjectilePool pool, TDirector director)
        {
            this.pool = pool ?? throw new ArgumentNullException(nameof(pool));
            this.director = director ?? throw new ArgumentNullException(nameof(director));
        }

        public Projectile GetReady(Transform spawnPoint)
        {
            Projectile projectile = pool.Acquire();
            if (projectile == null)
            {
                return null;
            }

            if (!director.TryConstruct(projectile, spawnPoint))
            {
                pool.Release(projectile);
                return null;
            }

            return projectile;
        }
    }
}
