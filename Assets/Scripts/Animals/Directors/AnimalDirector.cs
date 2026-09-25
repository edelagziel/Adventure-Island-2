using System;

public sealed class AnimalDirector<TAnimal>
    where TAnimal : Animal
{
    private readonly IAnimalBuilder<TAnimal> animalBuilder;

    public AnimalDirector(IAnimalBuilder<TAnimal> animalBuilder)
    {
        this.animalBuilder = animalBuilder
            ?? throw new ArgumentNullException(nameof(animalBuilder));
    }

    public TAnimal Construct()
    {
        return animalBuilder.Build();
    }
}
