using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace AdventureIsland.Combat
{
    [DisallowMultipleComponent]
    public sealed class WeaponInstaller : MonoBehaviour, IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register<WeaponLoadout>(Lifetime.Scoped)
                .AsSelf();
            builder.Register<WeaponController>(Lifetime.Scoped)
                .AsSelf();
        }
    }
}
