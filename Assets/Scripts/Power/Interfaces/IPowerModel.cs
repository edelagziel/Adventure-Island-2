public interface IPowerModel
{
    int CurrentPower { get; }

    int MinimumPower { get; }

    int MaximumPower { get; }

    bool AddPower(int amount);
    bool ReducePower(int amount);
    void ResetPower();
}
