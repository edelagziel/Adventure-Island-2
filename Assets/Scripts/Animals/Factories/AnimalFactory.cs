using System;

public sealed class AnimalFactory<TAnimal>
    where TAnimal : Animal
{
    private readonly AnimalDirector<TAnimal> animalDirector;

    public AnimalFactory(AnimalDirector<TAnimal> animalDirector)
    {
        this.animalDirector = animalDirector
            ?? throw new ArgumentNullException(nameof(animalDirector));
    }

    public TAnimal Create()
    {
        return animalDirector.Construct();
    }
}
