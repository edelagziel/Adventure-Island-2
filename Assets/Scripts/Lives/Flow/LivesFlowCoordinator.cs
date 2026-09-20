using System;

public sealed class LivesFlowCoordinator : IDisposable
{
    private readonly PowerController powerController;
    private readonly LivesController livesController;
    private readonly FruitProgressController fruitProgressController;
    private readonly PowerDrainRunner powerDrainRunner;

    public LivesFlowCoordinator(
        PowerController powerController,
        LivesController livesController,
        FruitProgressController fruitProgressController,
        PowerDrainRunner powerDrainRunner)
    {
        this.powerController = powerController
            ?? throw new ArgumentNullException(nameof(powerController));
        this.livesController = livesController
            ?? throw new ArgumentNullException(nameof(livesController));
        this.fruitProgressController = fruitProgressController
            ?? throw new ArgumentNullException(nameof(fruitProgressController));
        this.powerDrainRunner = powerDrainRunner
            ?? throw new ArgumentNullException(nameof(powerDrainRunner));

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
        powerController.Reset();
        powerDrainRunner.RestartDrainInterval();
    }

    private void ResetGame()
    {
        livesController.Reset();
        fruitProgressController.Reset();
        powerController.Reset();
        powerDrainRunner.RestartDrainInterval();
    }
}
