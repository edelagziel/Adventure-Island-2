using System;

public sealed class PowerModel : IPowerModel
{
    // Power values are supplied by the gameplay scope; the Model owns their rules thereafter.
    public PowerModel(int initialPower, int minimumPower, int maximumPower)
    {
        if (minimumPower > maximumPower)
        {
            throw new ArgumentException("Minimum Power cannot be greater than Maximum Power.");
        }

        if (initialPower < minimumPower || initialPower > maximumPower)
        {
            throw new ArgumentOutOfRangeException(
                nameof(initialPower),
                initialPower,
                "Initial Power must be inside the configured Power range.");
        }

        InitialPower = initialPower;
        MinimumPower = minimumPower;
        MaximumPower = maximumPower;
        CurrentPower = initialPower;
    }

    public int CurrentPower { get; private set; }

    public int InitialPower { get; }

    public int MinimumPower { get; }

    public int MaximumPower { get; }

    public bool AddPower(int amount)
    {
        ValidatePositiveAmount(amount);
        long requestedPower = (long)CurrentPower + amount;
        return SetPower(requestedPower);
    }

    public bool ReducePower(int amount)
    {
        ValidatePositiveAmount(amount);
        long requestedPower = (long)CurrentPower - amount;
        return SetPower(requestedPower);
    }

    private bool SetPower(long requestedPower)
    {
        int nextPower = (int)Math.Max(MinimumPower, Math.Min(MaximumPower, requestedPower));
        if (nextPower == CurrentPower)
        {
            return false;
        }
        CurrentPower = nextPower;
        return true;
    }

    private static void ValidatePositiveAmount(int amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                amount,
                "Power changes must use a positive amount.");
        }
    }
}
