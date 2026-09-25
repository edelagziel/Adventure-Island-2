using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[DisallowMultipleComponent]
public sealed class AnimalsInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private BlueAnimal blueAnimalPrefab;
    [SerializeField] private PlayerActiveAnimal playerActiveAnimal;

    public void Install(IContainerBuilder builder)
    {
        ValidateConfiguration();

        builder.RegisterComponent(playerActiveAnimal)
            .AsSelf();
        builder.Register<BlueAnimalBuilder>(Lifetime.Scoped)
            .WithParameter(nameof(blueAnimalPrefab), blueAnimalPrefab)
            .As<IAnimalBuilder<BlueAnimal>>();
        builder.Register(typeof(AnimalDirector<>), Lifetime.Scoped)
            .AsSelf();
        builder.Register(typeof(AnimalFactory<>), Lifetime.Scoped)
            .AsSelf();
        builder.Register(typeof(PlayerAnimalCollector<>), Lifetime.Scoped)
            .AsSelf();
    }

    private void ValidateConfiguration()
    {
        if (blueAnimalPrefab == null)
        {
            throw new InvalidOperationException(
                "AnimalsInstaller requires a Blue Animal prefab reference.");
        }

        if (playerActiveAnimal == null)
        {
            throw new InvalidOperationException(
                "AnimalsInstaller requires a PlayerActiveAnimal reference.");
        }
    }
}
