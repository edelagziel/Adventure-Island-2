using System;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public sealed class GameScreensView : MonoBehaviour, IGameScreensView
{
    [SerializeField] private GameObject gameplayHud;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject finalCompletePanel;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button gameOverNewGameButton;
    [SerializeField] private Button gameOverExitButton;
    [SerializeField] private Button finalCompleteNewGameButton;
    [SerializeField] private Button finalCompleteExitButton;
    [SerializeField] private Button mainMenuExitButton;

    public event Action StartGameRequested;
    public event Action NewGameRequested;
    public event Action ExitGameRequested;

    private void Awake()
    {
        ValidateConfiguration();
        startGameButton.onClick.AddListener(RequestStartGame);
        gameOverNewGameButton.onClick.AddListener(RequestNewGame);
        gameOverExitButton.onClick.AddListener(RequestExitGame);
        finalCompleteNewGameButton.onClick.AddListener(RequestNewGame);
        finalCompleteExitButton.onClick.AddListener(RequestExitGame);
        mainMenuExitButton.onClick.AddListener(RequestExitGame);
    }

    private void OnDestroy()
    {
        startGameButton.onClick.RemoveListener(RequestStartGame);
        gameOverNewGameButton.onClick.RemoveListener(RequestNewGame);
        gameOverExitButton.onClick.RemoveListener(RequestExitGame);
        finalCompleteNewGameButton.onClick.RemoveListener(RequestNewGame);
        finalCompleteExitButton.onClick.RemoveListener(RequestExitGame);
        mainMenuExitButton.onClick.RemoveListener(RequestExitGame);
    }

    public void Show(GameSessionState state)
    {
        gameplayHud.SetActive(state == GameSessionState.Playing);
        mainMenuPanel.SetActive(state == GameSessionState.MainMenu);
        gameOverPanel.SetActive(state == GameSessionState.GameOver);
        finalCompletePanel.SetActive(state == GameSessionState.FinalComplete);
    }

    private void RequestStartGame()
    {
        StartGameRequested?.Invoke();
    }

    private void RequestNewGame()
    {
        NewGameRequested?.Invoke();
    }

    private void RequestExitGame()
    {
        ExitGameRequested?.Invoke();
    }

    private void ValidateConfiguration()
    {
        if (gameplayHud == null || mainMenuPanel == null || gameOverPanel == null || finalCompletePanel == null ||
            startGameButton == null || gameOverNewGameButton == null || gameOverExitButton == null ||
            finalCompleteNewGameButton == null || finalCompleteExitButton == null || mainMenuExitButton == null)
        {
            throw new InvalidOperationException("GameScreensView requires all screen panels, HUD, and buttons.");
        }
    }
}
