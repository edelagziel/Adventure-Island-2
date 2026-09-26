using System;
using UnityEngine;

public sealed class GameSessionFlowController : IDisposable
{
    private readonly LivesFlowCoordinator livesFlowCoordinator;
    private readonly LivesController livesController;
    private readonly StageFlowController stageFlowController;
    private readonly IGameScreensView gameScreensView;
    private readonly GameplaySessionLock gameplaySessionLock;

    private bool isInitialized;

    public GameSessionFlowController(
        LivesFlowCoordinator livesFlowCoordinator,
        LivesController livesController,
        StageFlowController stageFlowController,
        IGameScreensView gameScreensView,
        GameplaySessionLock gameplaySessionLock)
    {
        this.livesFlowCoordinator = livesFlowCoordinator
            ?? throw new ArgumentNullException(nameof(livesFlowCoordinator));
        this.livesController = livesController
            ?? throw new ArgumentNullException(nameof(livesController));
        this.stageFlowController = stageFlowController
            ?? throw new ArgumentNullException(nameof(stageFlowController));
        this.gameScreensView = gameScreensView
            ?? throw new ArgumentNullException(nameof(gameScreensView));
        this.gameplaySessionLock = gameplaySessionLock
            ?? throw new ArgumentNullException(nameof(gameplaySessionLock));
    }

    public void Initialize()
    {
        if (isInitialized)
        {
            return;
        }

        isInitialized = true;
        livesFlowCoordinator.GameOverReached += ShowGameOver;
        stageFlowController.GameCompleted += ShowFinalComplete;
        gameScreensView.StartGameRequested += StartNewGame;
        gameScreensView.NewGameRequested += StartNewGame;
        gameScreensView.ExitGameRequested += ExitGame;

        ShowMainMenu();
    }

    public void Dispose()
    {
        if (!isInitialized)
        {
            return;
        }

        livesFlowCoordinator.GameOverReached -= ShowGameOver;
        stageFlowController.GameCompleted -= ShowFinalComplete;
        gameScreensView.StartGameRequested -= StartNewGame;
        gameScreensView.NewGameRequested -= StartNewGame;
        gameScreensView.ExitGameRequested -= ExitGame;
    }

    private void ShowMainMenu()
    {
        gameplaySessionLock.SetGameplayActive(false);
        gameScreensView.Show(GameSessionState.MainMenu);
    }

    private void StartNewGame()
    {
        gameplaySessionLock.SetGameplayActive(false);
        livesController.ResetState();
        stageFlowController.ResetToFirstStage();
        gameScreensView.Show(GameSessionState.Playing);
        gameplaySessionLock.SetGameplayActive(true);
    }

    private void ShowGameOver()
    {
        gameplaySessionLock.SetGameplayActive(false);
        gameScreensView.Show(GameSessionState.GameOver);
    }

    private void ShowFinalComplete()
    {
        gameplaySessionLock.SetGameplayActive(false);
        gameScreensView.Show(GameSessionState.FinalComplete);
    }

    private static void ExitGame()
    {
#if UNITY_EDITOR
        Debug.Log("Exit Game requested. Application.Quit() is ignored in the Unity Editor.");
#else
        Application.Quit();
#endif
    }
}
