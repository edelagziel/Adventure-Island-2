using System;

public sealed class LivesModel : ILivesModel
{
    private const int MinimumLives = 0;

    public LivesModel(int initialLives)
    {
        if (initialLives <= MinimumLives)
        {
            throw new ArgumentOutOfRangeException(
                nameof(initialLives),
                initialLives,
                "Initial Lives must be greater than zero.");
        }

        InitialLives = initialLives;
        CurrentLives = initialLives;
    }

    public int InitialLives { get; }

    public int CurrentLives { get; private set; }

    public bool LoseLife()
    {
        if (CurrentLives == MinimumLives)
        {
            return false;
        }

        CurrentLives--;
        return true;
    }

    public bool GainLife()
    {
        CurrentLives = checked(CurrentLives + 1);
        return true;
    }

    public bool ResetState()
    {
        if (CurrentLives == InitialLives)
        {
            return false;
        }

        CurrentLives = InitialLives;
        return true;
    }
}
