using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[DisallowMultipleComponent]
public sealed class LivesInstaller : LifetimeScope, IInstaller
{
    [Header("Lives")]
    [SerializeField] private LivesView livesView;
    [SerializeField, Min(1)] private int initialLives = 3;

    protected override void Configure(IContainerBuilder builder)
    {
        Install(builder);
    }

    public void Install(IContainerBuilder builder)
    {
        ValidateConfiguration();

        builder.Register<LivesModel>(Lifetime.Scoped)
            .WithParameter(nameof(initialLives), initialLives)
            .As<ILivesModel>();
        builder.RegisterComponent(livesView)
            .As<ILivesView>();
        builder.Register<LivesController>(Lifetime.Scoped)
            .AsSelf();

        // The controller is the root mediator, so only composition resolves it.
        builder.RegisterBuildCallback(container => container.Resolve<LivesController>());
    }

    private void ValidateConfiguration()
    {
        if (livesView == null)
        {
            throw new InvalidOperationException("LivesInstaller requires a LivesView reference.");
        }

        if (initialLives <= 0)
        {
            throw new InvalidOperationException("Initial Lives must be greater than zero.");
        }
    }
}
