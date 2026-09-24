using System;
using UnityEngine;

namespace AdventureIsland.Combat
{
    public sealed class HammerProjectileDirector : ProjectileDirector
    {
        private readonly IProjectileBuilder builder;

        public HammerProjectileDirector(IProjectileBuilder builder)
        {
            this.builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public override bool TryConstruct(Projectile projectile, Transform spawnPoint)
        {
            builder.Reset();
            builder.SetProduct(projectile);
            builder.SetSpawnPoint(spawnPoint);
            return builder.TryBuild();
        }
    }
}
