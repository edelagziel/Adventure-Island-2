using System;
using AdventureIsland.Combat;
using UnityEngine;

public sealed class RedFireProjectileDirector : ProjectileDirector
{
    private readonly IProjectileBuilder builder;

    public RedFireProjectileDirector(IProjectileBuilder builder)
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
