using System;

public sealed class BlueAnimalBuilder : IAnimalBuilder<BlueAnimal>
{
    private readonly BlueAnimal blueAnimalPrefab;

    public BlueAnimalBuilder(BlueAnimal blueAnimalPrefab)
    {
        this.blueAnimalPrefab = blueAnimalPrefab
            ?? throw new ArgumentNullException(nameof(blueAnimalPrefab));
    }

    public BlueAnimal Build()
    {
        return UnityEngine.Object.Instantiate(blueAnimalPrefab);
    }
}
