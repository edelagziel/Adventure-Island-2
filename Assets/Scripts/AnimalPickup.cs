using System;
using UnityEngine;
using VContainer;

[DisallowMultipleComponent]
public abstract class AnimalPickup<TAnimal> : PickUp
    where TAnimal : Animal
{
    private PlayerAnimalCollector<TAnimal> playerAnimalCollector;

    [Inject]
    public void Construct(
        PlayerAnimalCollector<TAnimal> injectedPlayerAnimalCollector)
    {
        playerAnimalCollector = injectedPlayerAnimalCollector
            ?? throw new ArgumentNullException(nameof(injectedPlayerAnimalCollector));
    }

    protected override void OnPickUp(GameObject player)
    {
        if (playerAnimalCollector == null)
        {
            throw new InvalidOperationException(
                "AnimalPickup requires PlayerAnimalCollector injection before collection.");
        }

        playerAnimalCollector.Collect();
    }
}
