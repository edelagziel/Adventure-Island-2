using System;
using AdventureIsland.Combat;
using AdventureIsland.Enemies;
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
    [SerializeField] private WeaponInstaller weaponInstaller;
    [SerializeField] private AnimalsInstaller animalsInstaller;
    [SerializeField] private EnemiesInstaller enemiesInstaller;
    [SerializeField] private ResetInstaller resetInstaller;

    protected override void Configure(IContainerBuilder builder)
    {
        ValidateConfiguration();

        powerInstaller.Install(builder);
        fruitProgressInstaller.Install(builder);
        livesInstaller.Install(builder);
        weaponInstaller.Install(builder);
        animalsInstaller.Install(builder);
        enemiesInstaller.Install(builder);
        resetInstaller.Install(builder);

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

        if (weaponInstaller == null)
        {
            throw new InvalidOperationException("GameLifetimeScope requires a WeaponInstaller reference.");
        }

        if (animalsInstaller == null)
        {
            throw new InvalidOperationException("GameLifetimeScope requires an AnimalsInstaller reference.");
        }

        if (enemiesInstaller == null)
        {
            throw new InvalidOperationException("GameLifetimeScope requires an EnemiesInstaller reference.");
        }

        if (resetInstaller == null)
        {
            throw new InvalidOperationException("GameLifetimeScope requires a ResetInstaller reference.");
        }
    }
}
