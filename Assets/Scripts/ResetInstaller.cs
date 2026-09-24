using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[DisallowMultipleComponent]
public sealed class ResetInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private PlayerResetter playerResetter;
    [SerializeField] private PickupResetter pickupResetter;

    public void Install(IContainerBuilder builder)
    {
        ValidateConfiguration();

        builder.RegisterComponent(playerResetter)
            .As<IPlayerResetter>();
        builder.RegisterComponent(pickupResetter)
            .As<IPickupResetter>();
    }

    private void ValidateConfiguration()
    {
        if (playerResetter == null)
        {
            throw new InvalidOperationException("ResetInstaller requires a PlayerResetter reference.");
        }

        if (pickupResetter == null)
        {
            throw new InvalidOperationException("ResetInstaller requires a PickupResetter reference.");
        }
    }
}
