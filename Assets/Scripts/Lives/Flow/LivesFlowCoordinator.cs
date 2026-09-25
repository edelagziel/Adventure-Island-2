using System;

public sealed class LivesFlowCoordinator : IDisposable
{
    private readonly PowerController powerController;
    private readonly LivesController livesController;
    private readonly FruitProgressController fruitProgressController;
    private readonly PowerDrainRunner powerDrainRunner;
    private readonly IPlayerResetter playerResetter;
    private readonly IPickupResetter pickupResetter;

    public LivesFlowCoordinator(
        PowerController powerController,
        LivesController livesController,
        FruitProgressController fruitProgressController,
        PowerDrainRunner powerDrainRunner,
        IPlayerResetter playerResetter,
        IPickupResetter pickupResetter)
    {
        this.powerController = powerController
            ?? throw new ArgumentNullException(nameof(powerController));
        this.livesController = livesController
            ?? throw new ArgumentNullException(nameof(livesController));
        this.fruitProgressController = fruitProgressController
            ?? throw new ArgumentNullException(nameof(fruitProgressController));
        this.powerDrainRunner = powerDrainRunner
            ?? throw new ArgumentNullException(nameof(powerDrainRunner));
        this.playerResetter = playerResetter
            ?? throw new ArgumentNullException(nameof(playerResetter));
        this.pickupResetter = pickupResetter
            ?? throw new ArgumentNullException(nameof(pickupResetter));

        this.fruitProgressController.FruitThresholdReached += OnFruitThresholdReached;
        this.powerController.PowerReachedMinimum += OnPowerReachedMinimum;
    }

    public void Dispose()
    {
        fruitProgressController.FruitThresholdReached -= OnFruitThresholdReached;
        powerController.PowerReachedMinimum -= OnPowerReachedMinimum;
    }

    private void OnFruitThresholdReached()
    {
        livesController.GainLife();
    }

    private void OnPowerReachedMinimum()
    {
        if (!livesController.LoseLife())
        {
            return;
        }

        if (livesController.CurrentLives > 0)
        {
            RestartCurrentAttempt();
            return;
        }

        ResetGame();
    }

    private void RestartCurrentAttempt()
    {
        ResetPlayerToInitialSpawn();
        ResetPowerAndDrain();
    }

    private void ResetGame()
    {
        ResetPlayerToInitialSpawn();
        pickupResetter.ReactivatePickups();
        livesController.ResetState();
        fruitProgressController.ResetState();
        ResetPowerAndDrain();
    }

    private void ResetPlayerToInitialSpawn()
    {
        playerResetter.ResetToInitialSpawn();
    }

    private void ResetPowerAndDrain()
    {
        powerController.ResetState();
        powerDrainRunner.ResetState();
    }
}
