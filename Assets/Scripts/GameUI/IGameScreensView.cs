using System;

public interface IGameScreensView
{
    event Action StartGameRequested;
    event Action NewGameRequested;
    event Action ExitGameRequested;

    void Show(GameSessionState state);
}
