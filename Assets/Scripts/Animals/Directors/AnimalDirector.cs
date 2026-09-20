using System;

public sealed class AnimalDirector
{
    private readonly IAnimalBuilder animalBuilder;

    public AnimalDirector(IAnimalBuilder animalBuilder)
    {
        this.animalBuilder = animalBuilder
            ?? throw new ArgumentNullException(nameof(animalBuilder));
    }

    public void ConstructAnimal(AnimalDefinition definition)
    {
        animalBuilder.BuildAnimalPrefab(definition);
    }

    public IAnimal GetAnimal()
    {
        return animalBuilder.GetAnimal();
    }
}
