using System;

public sealed class PowerController : IResettable
{
    private readonly IPowerModel model;
    private readonly IPowerView view;

    public event Action PowerReachedMinimum;

    public PowerController(IPowerModel model, IPowerView view)
    {
        this.model = model ?? throw new ArgumentNullException(nameof(model));
        this.view = view ?? throw new ArgumentNullException(nameof(view));

        UpdateView();
    }

    public bool AddPower(int amount)
    {
        bool changed = model.AddPower(amount);

        if (changed)
        {
            UpdateView();
        }

        return changed;
    }

    public bool ReducePower(int amount)
    {
        bool changed = model.ReducePower(amount);

        if (changed)
        {
            UpdateView();

            if (model.CurrentPower == model.MinimumPower)
            {
                PowerReachedMinimum?.Invoke();
            }
        }

        return changed;
    }

    public bool Reset()
    {
        bool changed = model.Reset();

        if (changed)
        {
            UpdateView();
        }

        return changed;
    }

    private void UpdateView()
    {
        view.UpdatePowerDisplay(
            model.CurrentPower,
            model.MaximumPower
        );
    }
}
