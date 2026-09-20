public interface ILivesModel : IResettable
{
    int CurrentLives { get; }

    bool LoseLife();
    bool GainLife();
}
