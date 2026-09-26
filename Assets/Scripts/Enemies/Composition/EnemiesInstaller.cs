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
            builder.Register<EnemyBuilder>(Lifetime.Scoped)
                .As<IEnemyBuilder>();
            builder.Register<EnemyDirector>(Lifetime.Scoped)
                .AsSelf();
            builder.Register<EnemyFactory>(Lifetime.Scoped)
                .AsSelf();
        }
    }
}
