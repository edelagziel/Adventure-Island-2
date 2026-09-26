using System;
using AdventureIsland.Combat;
using AdventureIsland.Enemies;
using AdventureIsland.Fairy;
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
    [SerializeField] private FairyInstaller fairyInstaller;
    [SerializeField] private ResetInstaller resetInstaller;
    [SerializeField] private StageInstaller stageInstaller;

    protected override void Configure(IContainerBuilder builder)
    {
        ValidateConfiguration();

        powerInstaller.Install(builder);
        fruitProgressInstaller.Install(builder);
        livesInstaller.Install(builder);
        weaponInstaller.Install(builder);
        animalsInstaller.Install(builder);
        enemiesInstaller.Install(builder);
        fairyInstaller.Install(builder);
        resetInstaller.Install(builder);
        stageInstaller.Install(builder);

        builder.Register<LivesFlowCoordinator>(Lifetime.Scoped)
            .AsSelf()
            .As<IPlayerFailureHandler>();
        builder.RegisterBuildCallback(container =>
            container.Resolve<LivesFlowCoordinator>());
        builder.RegisterBuildCallback(container =>
            container.Resolve<StageFlowController>().Initialize());
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

        if (fairyInstaller == null)
        {
            throw new InvalidOperationException("GameLifetimeScope requires a FairyInstaller reference.");
        }

        if (resetInstaller == null)
        {
            throw new InvalidOperationException("GameLifetimeScope requires a ResetInstaller reference.");
        }

        if (stageInstaller == null)
        {
            throw new InvalidOperationException("GameLifetimeScope requires a StageInstaller reference.");
        }
    }
}
