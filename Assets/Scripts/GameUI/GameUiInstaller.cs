using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[DisallowMultipleComponent]
public sealed class GameUiInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private GameScreensView gameScreensView;
    [SerializeField] private GameplaySessionLock gameplaySessionLock;
    [SerializeField] private WeaponIndicatorView weaponIndicatorView;
    [SerializeField] private AnimalIndicatorView animalIndicatorView;

    public void Install(IContainerBuilder builder)
    {
        ValidateConfiguration();

        builder.RegisterComponent(gameScreensView)
            .As<IGameScreensView>();
        builder.RegisterComponent(gameplaySessionLock)
            .AsSelf();
        builder.RegisterComponent(weaponIndicatorView)
            .As<IWeaponIndicatorView>();
        builder.RegisterComponent(animalIndicatorView)
            .As<IAnimalIndicatorView>();
        builder.Register<GameSessionFlowController>(Lifetime.Scoped)
            .AsSelf();
        builder.Register<WeaponIndicatorController>(Lifetime.Scoped)
            .AsSelf();
        builder.Register<AnimalIndicatorController>(Lifetime.Scoped)
            .AsSelf();
    }

    private void ValidateConfiguration()
    {
        if (gameScreensView == null || gameplaySessionLock == null ||
            weaponIndicatorView == null || animalIndicatorView == null)
        {
            throw new InvalidOperationException(
                "GameUiInstaller requires its screen, session lock, weapon, and animal view references.");
        }
    }
}
