using System;
using UnityEngine;
using VContainer;

[DisallowMultipleComponent]
public sealed class AnimalPickup : PickUp
{
    [SerializeReference] private AnimalDefinition animalDefinition;

    private AnimalFactory animalFactory;

    [Inject]
    public void Construct(AnimalFactory injectedAnimalFactory)
    {
        animalFactory = injectedAnimalFactory
            ?? throw new ArgumentNullException(nameof(injectedAnimalFactory));
    }

    protected override void OnPickUp(GameObject player)
    {
        if (animalFactory == null)
        {
            throw new InvalidOperationException(
                "AnimalPickup requires AnimalFactory injection before collection.");
        }

        PlayerAnimalMount playerAnimalMount = player.GetComponent<PlayerAnimalMount>();
        if (playerAnimalMount == null)
        {
            throw new InvalidOperationException(
                "AnimalPickup requires PlayerAnimalMount on the Player GameObject.");
        }

        playerAnimalMount.SetActiveAnimal(animalFactory.Create(animalDefinition));
    }
}
