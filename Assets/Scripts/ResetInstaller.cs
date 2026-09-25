using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[DisallowMultipleComponent]
public sealed class ResetInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private PlayerResetter playerResetter;

    public void Install(IContainerBuilder builder)
    {
        ValidateConfiguration();

        builder.RegisterComponent(playerResetter)
            .As<IPlayerResetter>();
    }

    private void ValidateConfiguration()
    {
        if (playerResetter == null)
        {
            throw new InvalidOperationException("ResetInstaller requires a PlayerResetter reference.");
        }

    }
}
