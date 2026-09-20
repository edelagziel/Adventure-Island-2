using System;
using UnityEngine;

[Serializable]
public sealed class BlueAnimalDefinition : AnimalDefinition
{
    public BlueAnimalDefinition()
    {
    }

    public BlueAnimalDefinition(MonoBehaviour animalPrefab)
        : base(animalPrefab)
    {
    }
}
