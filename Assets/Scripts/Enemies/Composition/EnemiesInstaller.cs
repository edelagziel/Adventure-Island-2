using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace AdventureIsland.Enemies
{
    [DisallowMultipleComponent]
    public sealed class EnemiesInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private Transform player;

        public void Install(IContainerBuilder builder)
        {
            if (player == null)
            {
                throw new System.InvalidOperationException(
                    "EnemiesInstaller requires a Player Transform reference.");
            }

            builder.RegisterInstance(player)
                .AsSelf();
            builder.Register<EnemyBuilder>(Lifetime.Scoped)
                .As<IEnemyBuilder>();
            builder.Register<EnemyDirector>(Lifetime.Scoped)
                .AsSelf();
            builder.Register<EnemyFactory>(Lifetime.Scoped)
                .AsSelf();
        }
    }
}
