using System;
using AdventureIsland.Combat;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[DisallowMultipleComponent]
public sealed class AnimalsInstaller : MonoBehaviour, IInstaller
{
    private const string RedFirePoolKey = "RedAnimalFireProjectilePool";

    [SerializeField] private BlueAnimal blueAnimalPrefab;
    [SerializeField] private RedAnimal redAnimalPrefab;
    [SerializeField] private GreenAnimal greenAnimalPrefab;
    [SerializeField] private Projectile redFireProjectilePrefab;
    [SerializeField] private PlayerActiveAnimal playerActiveAnimal;

    public void Install(IContainerBuilder builder)
    {
        ValidateConfiguration();

        builder.RegisterComponent(playerActiveAnimal)
            .AsSelf();
        builder.Register<BlueAnimalBuilder>(Lifetime.Scoped)
            .WithParameter(nameof(blueAnimalPrefab), blueAnimalPrefab)
            .As<IAnimalBuilder<BlueAnimal>>();
        builder.Register<RedAnimalBuilder>(Lifetime.Scoped)
            .WithParameter(nameof(redAnimalPrefab), redAnimalPrefab)
            .As<IAnimalBuilder<RedAnimal>>();
        builder.Register<GreenAnimalBuilder>(Lifetime.Scoped)
            .WithParameter(nameof(greenAnimalPrefab), greenAnimalPrefab)
            .As<IAnimalBuilder<GreenAnimal>>();
        builder.Register(typeof(AnimalDirector<>), Lifetime.Scoped)
            .AsSelf();
        builder.Register(typeof(AnimalFactory<>), Lifetime.Scoped)
            .AsSelf();
        builder.Register(typeof(PlayerAnimalCollector<>), Lifetime.Scoped)
            .AsSelf();
        builder.Register<RedFireProjectileBuilder>(Lifetime.Transient)
            .AsSelf();
        builder.Register<RedFireProjectileDirector>(Lifetime.Scoped)
            .WithParameter("builder", resolver =>
                resolver.Resolve<RedFireProjectileBuilder>())
            .AsSelf();
        builder.Register<ProjectilePool>(Lifetime.Scoped)
            .Keyed(RedFirePoolKey)
            .WithParameter("prefab", redFireProjectilePrefab)
            .AsSelf();
        builder.Register<ProjectileProvider<RedFireProjectileDirector>>(Lifetime.Scoped)
            .WithParameter("pool", resolver =>
                resolver.Resolve<ProjectilePool>(RedFirePoolKey))
            .AsSelf();
    }

    private void ValidateConfiguration()
    {
        if (blueAnimalPrefab == null)
        {
            throw new InvalidOperationException(
                "AnimalsInstaller requires a Blue Animal prefab reference.");
        }

        if (redAnimalPrefab == null)
        {
            throw new InvalidOperationException(
                "AnimalsInstaller requires a Red Animal prefab reference.");
        }

        if (greenAnimalPrefab == null)
        {
            throw new InvalidOperationException(
                "AnimalsInstaller requires a Green Animal prefab reference.");
        }

        if (redFireProjectilePrefab == null)
        {
            throw new InvalidOperationException(
                "AnimalsInstaller requires a Red fire projectile prefab reference.");
        }

        if (playerActiveAnimal == null)
        {
            throw new InvalidOperationException(
                "AnimalsInstaller requires a PlayerActiveAnimal reference.");
        }
    }
}
