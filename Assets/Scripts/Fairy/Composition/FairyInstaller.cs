using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace AdventureIsland.Fairy
{
    [DisallowMultipleComponent]
    public sealed class FairyInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private FairyProtection fairyProtection;

        public void Install(IContainerBuilder builder)
        {
            if (fairyProtection == null)
            {
                throw new InvalidOperationException(
                    "FairyInstaller requires the Player FairyProtection component.");
            }

            builder.RegisterComponent(fairyProtection)
                .AsSelf()
                .As<IPlayerProtectionState>()
                .As<IResettable>();
        }
    }
}
