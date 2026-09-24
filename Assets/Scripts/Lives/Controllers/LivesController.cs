using System;

public sealed class LivesController : IResettable
{
    private readonly ILivesModel model;
    private readonly ILivesView view;

    public LivesController(ILivesModel model, ILivesView view)
    {
        this.model = model ?? throw new ArgumentNullException(nameof(model));
        this.view = view ?? throw new ArgumentNullException(nameof(view));

        UpdateView();
    }

    public bool LoseLife()
    {
        bool changed = model.LoseLife();

        if (changed)
        {
            UpdateView();
        }

        return changed;
    }

    public int CurrentLives => model.CurrentLives;

    public bool GainLife()
    {
        bool changed = model.GainLife();

        if (changed)
        {
            UpdateView();
        }

        return changed;
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
        view.UpdateLivesDisplay(model.CurrentLives);
    }
}
