using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace AdventureIsland.Enemies
{
    [DisallowMultipleComponent]
    public sealed class EnemiesInstaller : MonoBehaviour, IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register(typeof(EnemyDirector<>), Lifetime.Scoped)
                .AsSelf();
            builder.Register(typeof(EnemyFactory<>), Lifetime.Scoped)
                .AsSelf();
        }
    }
}
