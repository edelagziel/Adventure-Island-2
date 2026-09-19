using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[DisallowMultipleComponent]
public sealed class FruitProgressInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private FruitProgressView fruitProgressView;
    [SerializeField, Min(1)] private int fruitThreshold = 20;

    public void Install(IContainerBuilder builder)
    {
        ValidateConfiguration();

        builder.Register<FruitProgressModel>(Lifetime.Scoped)
            .WithParameter(nameof(fruitThreshold), fruitThreshold)
            .As<IFruitProgressModel>();
        builder.RegisterComponent(fruitProgressView)
            .As<IFruitProgressView>();
        builder.Register<FruitProgressController>(Lifetime.Scoped)
            .AsSelf();

        builder.RegisterBuildCallback(container => container.Resolve<FruitProgressController>());
    }

    private void ValidateConfiguration()
    {
        if (fruitProgressView == null)
        {
            throw new InvalidOperationException("FruitProgressInstaller requires a FruitProgressView reference.");
        }

        if (fruitThreshold <= 0)
        {
            throw new InvalidOperationException("Fruit progress threshold must be greater than zero.");
        }
    }
}
