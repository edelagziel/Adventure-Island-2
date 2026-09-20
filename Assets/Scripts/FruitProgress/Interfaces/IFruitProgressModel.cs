public interface IFruitProgressModel : IResettable
{
    int CurrentFruitCount { get; }

    int FruitThreshold { get; }

    bool CollectFruit();
}
