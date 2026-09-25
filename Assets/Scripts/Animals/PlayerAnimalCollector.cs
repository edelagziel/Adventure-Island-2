using System;

public sealed class PlayerAnimalCollector<TAnimal>
    where TAnimal : Animal
{
    private readonly AnimalFactory<TAnimal> animalFactory;
    private readonly PlayerActiveAnimal playerActiveAnimal;

    public PlayerAnimalCollector(
        AnimalFactory<TAnimal> animalFactory,
        PlayerActiveAnimal playerActiveAnimal)
    {
        this.animalFactory = animalFactory
            ?? throw new ArgumentNullException(nameof(animalFactory));
        this.playerActiveAnimal = playerActiveAnimal
            ?? throw new ArgumentNullException(nameof(playerActiveAnimal));
    }

    public void Collect()
    {
        playerActiveAnimal.SetActiveAnimal(animalFactory.Create());
    }
}
