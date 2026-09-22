using System;

public sealed class AnimalFactory
{
    private readonly AnimalDirector animalDirector;

    public AnimalFactory(AnimalDirector animalDirector)
    {
        this.animalDirector = animalDirector
            ?? throw new ArgumentNullException(nameof(animalDirector));
    }

    public IAnimal Create(AnimalDefinition definition)
    {
        animalDirector.ConstructAnimal(definition);
        return animalDirector.GetAnimal();
    }
}
