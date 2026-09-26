using System;
using UnityEngine;

public sealed class LivesFlowCoordinator : IDisposable, IPlayerFailureHandler
{
    private readonly PowerController powerController;
    private readonly LivesController livesController;
    private readonly FruitProgressController fruitProgressController;
    private readonly StageFlowController stageFlowController;
    private bool isHandlingPlayerFailure;
    private int lastHandledFailureFrame = -1;

    public LivesFlowCoordinator(
        PowerController powerController,
        LivesController livesController,
        FruitProgressController fruitProgressController,
        StageFlowController stageFlowController)
    {
        this.powerController = powerController ?? throw new ArgumentNullException(nameof(powerController));
        this.livesController = livesController ?? throw new ArgumentNullException(nameof(livesController));
        this.fruitProgressController = fruitProgressController ?? throw new ArgumentNullException(nameof(fruitProgressController));
        this.stageFlowController = stageFlowController ?? throw new ArgumentNullException(nameof(stageFlowController));

        this.fruitProgressController.FruitThresholdReached += OnFruitThresholdReached;
        this.powerController.PowerReachedMinimum += OnPowerReachedMinimum;
    }

    public void Dispose()
    {
        fruitProgressController.FruitThresholdReached -= OnFruitThresholdReached;
        powerController.PowerReachedMinimum -= OnPowerReachedMinimum;
    }

    public bool TryHandlePlayerFailure()
    {
        if (isHandlingPlayerFailure || lastHandledFailureFrame == Time.frameCount)
        {
            return false;
        }

        isHandlingPlayerFailure = true;
        lastHandledFailureFrame = Time.frameCount;

        try
        {
            if (!livesController.LoseLife())
            {
                return false;
            }

            if (livesController.CurrentLives > 0)
            {
                stageFlowController.RestartCurrentStage();
                return true;
            }

            livesController.ResetState();
            stageFlowController.ResetToFirstStage();
            return true;
        }
        finally
        {
            isHandlingPlayerFailure = false;
        }
    }

    private void OnFruitThresholdReached()
    {
        livesController.GainLife();
    }

    private void OnPowerReachedMinimum()
    {
        TryHandlePlayerFailure();
    }
}
