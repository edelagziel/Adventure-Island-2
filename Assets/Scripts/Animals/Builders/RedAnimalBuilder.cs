using System;
using AdventureIsland.Combat;
using UnityEngine;

public sealed class RedAnimalBuilder : IAnimalBuilder<RedAnimal>
{
    private readonly RedAnimal redAnimalPrefab;
    private readonly ProjectileProvider<RedFireProjectileDirector> projectileProvider;

    public RedAnimalBuilder(
        RedAnimal redAnimalPrefab,
        ProjectileProvider<RedFireProjectileDirector> projectileProvider)
    {
        this.redAnimalPrefab = redAnimalPrefab
            ?? throw new ArgumentNullException(nameof(redAnimalPrefab));
        this.projectileProvider = projectileProvider
            ?? throw new ArgumentNullException(nameof(projectileProvider));
    }

    public RedAnimal Build()
    {
        RedAnimal animal = UnityEngine.Object.Instantiate(redAnimalPrefab);
        animal.Configure(projectileProvider);
        return animal;
    }
}
