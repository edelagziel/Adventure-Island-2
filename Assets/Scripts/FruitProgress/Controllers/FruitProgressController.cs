using System;

public sealed class FruitProgressController : IResettable
{
    private readonly IFruitProgressModel model;
    private readonly IFruitProgressView view;

    public FruitProgressController(IFruitProgressModel model, IFruitProgressView view)
    {
        this.model = model ?? throw new ArgumentNullException(nameof(model));
        this.view = view ?? throw new ArgumentNullException(nameof(view));

        UpdateView();
    }

    public event Action FruitThresholdReached;
    public event Action FruitCollected;

    public void CollectFruit()
    {
        bool thresholdReached = model.CollectFruit();

        UpdateView();

        if (thresholdReached)
        {
            FruitThresholdReached?.Invoke();
        }

        FruitCollected?.Invoke();
    }

    public bool ResetState()
    {
        bool changed = model.ResetState();

        if (changed)
        {
            UpdateView();
        }

        return changed;
    }

    private void UpdateView()
    {
        view.UpdateFruitProgress(
            model.CurrentFruitCount,
            model.FruitThreshold
        );
    }
}
