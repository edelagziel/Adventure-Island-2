using System;
using UnityEngine;

[Serializable]
public abstract class AnimalDefinition
{
    [SerializeField] private MonoBehaviour animalPrefab;

    protected AnimalDefinition()
    {
    }

    protected AnimalDefinition(MonoBehaviour animalPrefab)
    {
        this.animalPrefab = animalPrefab
            ?? throw new ArgumentNullException(nameof(animalPrefab));
    }

    public MonoBehaviour AnimalPrefab => animalPrefab;
}
