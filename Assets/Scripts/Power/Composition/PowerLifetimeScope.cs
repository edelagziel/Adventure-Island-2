using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[DisallowMultipleComponent]
public sealed class PowerLifetimeScope : LifetimeScope
{
    [Header("Power")]
    [SerializeField] private PowerView powerView;
    [SerializeField, Min(0)] private int initialPower = 20;
    [SerializeField, Min(0)] private int minimumPower;
    [SerializeField, Min(0)] private int maximumPower = 30;

    protected override void Configure(IContainerBuilder builder)
    {
        ValidateConfiguration();

        builder.Register<PowerModel>(Lifetime.Scoped)
            .WithParameter(nameof(initialPower), initialPower)
            .WithParameter(nameof(minimumPower), minimumPower)
            .WithParameter(nameof(maximumPower), maximumPower)
            .As<IPowerModel>();

        builder.RegisterComponent(powerView)
            .As<IPowerView>();
        builder.Register<PowerController>(Lifetime.Scoped)
            .AsSelf();

        // The controller is the root mediator, so only the Composition Root resolves it.
        builder.RegisterBuildCallback(container => container.Resolve<PowerController>());
    }

    private void ValidateConfiguration()
    {
        if (powerView == null)
        {
            throw new InvalidOperationException("PowerLifetimeScope requires a PowerView reference.");
        }

        if (minimumPower > maximumPower)
        {
            throw new InvalidOperationException("Minimum Power cannot be greater than Maximum Power.");
        }

        if (initialPower < minimumPower || initialPower > maximumPower)
        {
            throw new InvalidOperationException("Initial Power must be inside the configured Power range.");
        }
    }
}
