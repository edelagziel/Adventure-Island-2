using System;

public sealed class GreenAnimalBuilder : IAnimalBuilder<GreenAnimal>
{
    private readonly GreenAnimal greenAnimalPrefab;

    public GreenAnimalBuilder(GreenAnimal greenAnimalPrefab)
    {
        this.greenAnimalPrefab = greenAnimalPrefab
            ?? throw new ArgumentNullException(nameof(greenAnimalPrefab));
    }

    public GreenAnimal Build()
    {
        return UnityEngine.Object.Instantiate(greenAnimalPrefab);
    }
}
