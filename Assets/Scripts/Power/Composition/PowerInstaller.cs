using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[DisallowMultipleComponent]
public sealed class PowerInstaller : MonoBehaviour, IInstaller
{
    [Header("Power")]
    [SerializeField] private PowerView powerView;
    [SerializeField, Min(0)] private int initialPower = 20;
    [SerializeField, Min(0)] private int minimumPower;
    [SerializeField, Min(0)] private int maximumPower = 30;

    [Header("Power Drain")]
    [SerializeField] private PowerDrainRunner powerDrainRunner;
    [SerializeField, Min(1)] private int drainAmount = 1;
    [SerializeField, Min(0.01f)] private float drainIntervalSeconds = 5f;

    public void Install(IContainerBuilder builder)
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
        builder.Register<PlayerDamageController>(Lifetime.Scoped)
            .AsSelf()
            .As<IPlayerDamageReceiver>();
        builder.RegisterComponent(powerDrainRunner)
            .WithParameter(nameof(drainAmount), drainAmount)
            .WithParameter(nameof(drainIntervalSeconds), drainIntervalSeconds)
            .AsSelf();

        // The controller is the root mediator, so only the Composition Root resolves it.
        builder.RegisterBuildCallback(container => container.Resolve<PowerController>());
    }

    private void ValidateConfiguration()
    {
        if (powerView == null)
        {
            throw new InvalidOperationException("PowerInstaller requires a PowerView reference.");
        }

        if (powerDrainRunner == null)
        {
            throw new InvalidOperationException("PowerInstaller requires a PowerDrainRunner reference.");
        }

        if (minimumPower > maximumPower)
        {
            throw new InvalidOperationException("Minimum Power cannot be greater than Maximum Power.");
        }

        if (initialPower < minimumPower || initialPower > maximumPower)
        {
            throw new InvalidOperationException("Initial Power must be inside the configured Power range.");
        }

        if (drainAmount <= 0)
        {
            throw new InvalidOperationException("Power drain amount must be greater than zero.");
        }

        if (drainIntervalSeconds <= 0f)
        {
            throw new InvalidOperationException("Power drain interval must be greater than zero.");
        }
    }
}
