using System;
using UnityEngine;

[Serializable]
public sealed class GreenAnimalDefinition : AnimalDefinition
{
    public GreenAnimalDefinition()
    {
    }

    public GreenAnimalDefinition(MonoBehaviour animalPrefab)
        : base(animalPrefab)
    {
    }
}
