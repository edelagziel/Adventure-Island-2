public interface IFruitProgressModel
{
    int CurrentFruitCount { get; }

    int FruitThreshold { get; }

    bool CollectFruit();
}
