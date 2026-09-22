using System;
using UnityEngine;

namespace AdventureIsland.Combat
{
    public sealed class HammerProjectileDirector : ProjectileDirector
    {
        private readonly HammerProjectileBuilder builder;

        public HammerProjectileDirector(HammerProjectileBuilder builder)
        {
            this.builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public override bool TryConstruct(Projectile projectile, Transform spawnPoint)
        {
            builder.Reset();
            builder.SetProduct(projectile as HammerProjectile);
            builder.SetSpawnPoint(spawnPoint);
            return builder.TryBuild();
        }
    }
}
