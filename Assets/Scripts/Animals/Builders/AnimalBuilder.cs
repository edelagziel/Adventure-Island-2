using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public sealed class AnimalBuilder : IAnimalBuilder
{
    private readonly IObjectResolver objectResolver;
    private IAnimal animal;

    public AnimalBuilder(IObjectResolver objectResolver)
    {
        this.objectResolver = objectResolver
            ?? throw new ArgumentNullException(nameof(objectResolver));
    }

    public void BuildAnimalPrefab(AnimalDefinition definition)
    {
        if (definition == null)
        {
            throw new ArgumentNullException(nameof(definition));
        }

        MonoBehaviour animalPrefab = definition.AnimalPrefab;
        if (animalPrefab == null)
        {
            throw new InvalidOperationException(
                "AnimalDefinition requires an Animal prefab component.");
        }

        if (!(animalPrefab is IAnimal))
        {
            throw new InvalidOperationException(
                "AnimalDefinition prefab component must implement IAnimal.");
        }

        MonoBehaviour animalInstance = objectResolver.Instantiate(animalPrefab);
        animal = animalInstance as IAnimal
            ?? throw new InvalidOperationException(
                "The created Animal instance must implement IAnimal.");
    }

    public IAnimal GetAnimal()
    {
        return animal ?? throw new InvalidOperationException(
            "AnimalBuilder must build an Animal before it can return one.");
    }
}
