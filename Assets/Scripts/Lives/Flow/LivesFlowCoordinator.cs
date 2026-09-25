using System;

public sealed class LivesFlowCoordinator : IDisposable
{
    private readonly PowerController powerController;
    private readonly LivesController livesController;
    private readonly FruitProgressController fruitProgressController;
    private readonly StageFlowController stageFlowController;

    public LivesFlowCoordinator(
        PowerController powerController,
        LivesController livesController,
        FruitProgressController fruitProgressController,
        StageFlowController stageFlowController)
    {
        this.powerController = powerController
            ?? throw new ArgumentNullException(nameof(powerController));
        this.livesController = livesController
            ?? throw new ArgumentNullException(nameof(livesController));
        this.fruitProgressController = fruitProgressController
            ?? throw new ArgumentNullException(nameof(fruitProgressController));
        this.stageFlowController = stageFlowController
            ?? throw new ArgumentNullException(nameof(stageFlowController));

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
            stageFlowController.RestartCurrentStage();
            return;
        }

        livesController.ResetState();
        stageFlowController.ResetToFirstStage();
    }
}
