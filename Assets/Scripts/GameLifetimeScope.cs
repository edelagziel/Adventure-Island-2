using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[DisallowMultipleComponent]
public sealed class GameLifetimeScope : LifetimeScope
{
    [Header("Feature Composition")]
    [SerializeField] private PowerInstaller powerInstaller;
    [SerializeField] private FruitProgressInstaller fruitProgressInstaller;
    [SerializeField] private LivesInstaller livesInstaller;

    protected override void Configure(IContainerBuilder builder)
    {
        ValidateConfiguration();

        powerInstaller.Install(builder);
        fruitProgressInstaller.Install(builder);
        livesInstaller.Install(builder);

        builder.Register<LivesFlowCoordinator>(Lifetime.Scoped)
            .AsSelf();
        builder.RegisterBuildCallback(container =>
            container.Resolve<LivesFlowCoordinator>());
    }

    private void ValidateConfiguration()
    {
        if (powerInstaller == null)
        {
            throw new InvalidOperationException("GameLifetimeScope requires a PowerInstaller reference.");
        }

        if (fruitProgressInstaller == null)
        {
            throw new InvalidOperationException("GameLifetimeScope requires a FruitProgressInstaller reference.");
        }

        if (livesInstaller == null)
        {
            throw new InvalidOperationException("GameLifetimeScope requires a LivesInstaller reference.");
        }
    }
}
