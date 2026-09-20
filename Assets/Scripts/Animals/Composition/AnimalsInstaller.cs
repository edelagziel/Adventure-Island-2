using UnityEngine;
using VContainer;
using VContainer.Unity;

[DisallowMultipleComponent]
public sealed class AnimalsInstaller : MonoBehaviour, IInstaller
{
    public void Install(IContainerBuilder builder)
    {
        builder.Register<AnimalBuilder>(Lifetime.Scoped)
            .As<IAnimalBuilder>();
        builder.Register<AnimalDirector>(Lifetime.Scoped)
            .AsSelf();
        builder.Register<AnimalFactory>(Lifetime.Scoped)
            .AsSelf();
    }
}
