using UnityEngine;
using VContainer;
using VContainer.Unity;

[DisallowMultipleComponent]
public sealed class AnimalsInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private PlayerAnimalMount playerAnimalMount;

    public void Install(IContainerBuilder builder)
    {
        if (playerAnimalMount == null)
        {
            throw new System.InvalidOperationException(
                "AnimalsInstaller requires a PlayerAnimalMount reference.");
        }

        builder.Register<AnimalBuilder>(Lifetime.Scoped)
            .As<IAnimalBuilder>();
        builder.Register<AnimalDirector>(Lifetime.Scoped)
            .AsSelf();
        builder.Register<AnimalFactory>(Lifetime.Scoped)
            .AsSelf();
        builder.RegisterComponent(playerAnimalMount)
            .AsSelf();
    }
}
