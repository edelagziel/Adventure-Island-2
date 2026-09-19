public interface ILivesModel
{
    int CurrentLives { get; }

    bool LoseLife();
    bool GainLife();
}
