using System;
using UnityEngine;

[Serializable]
public sealed class RedAnimalDefinition : AnimalDefinition
{
    public RedAnimalDefinition()
    {
    }

    public RedAnimalDefinition(MonoBehaviour animalPrefab)
        : base(animalPrefab)
    {
    }
}
