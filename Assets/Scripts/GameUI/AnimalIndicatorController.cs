using System;

public sealed class AnimalIndicatorController : IDisposable
{
    private readonly PlayerActiveAnimal playerActiveAnimal;
    private readonly IAnimalIndicatorView animalIndicatorView;

    public AnimalIndicatorController(
        PlayerActiveAnimal playerActiveAnimal,
        IAnimalIndicatorView animalIndicatorView)
    {
        this.playerActiveAnimal = playerActiveAnimal
            ?? throw new ArgumentNullException(nameof(playerActiveAnimal));
        this.animalIndicatorView = animalIndicatorView
            ?? throw new ArgumentNullException(nameof(animalIndicatorView));

        playerActiveAnimal.ActiveAnimalChanged += UpdateView;
        UpdateView();
    }

    public void Dispose()
    {
        playerActiveAnimal.ActiveAnimalChanged -= UpdateView;
    }

    private void UpdateView()
    {
        animalIndicatorView.UpdateAnimalDisplay(playerActiveAnimal.ActiveAnimal);
    }
}
