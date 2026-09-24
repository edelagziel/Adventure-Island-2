using System;

public sealed class FruitProgressModel : IFruitProgressModel
{
    public FruitProgressModel(int fruitThreshold)
    {
        if (fruitThreshold <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(fruitThreshold),
                fruitThreshold,
                "Fruit progress threshold must be greater than zero.");
        }

        FruitThreshold = fruitThreshold;
    }

    public int CurrentFruitCount { get; private set; }

    public int FruitThreshold { get; }

    public bool CollectFruit()
    {
        CurrentFruitCount++;

        if (CurrentFruitCount < FruitThreshold)
        {
            return false;
        }

        CurrentFruitCount = 0;
        return true;
    }

    public bool ResetState()
    {
        if (CurrentFruitCount == 0)
        {
            return false;
        }

        CurrentFruitCount = 0;
        return true;
    }
}
