using System;
using UnityEngine;
using VContainer;

[DisallowMultipleComponent]
public sealed class FruitPickup : PickUp
{
    [SerializeField, Min(1)] private int powerAmount = 1;

    private PowerController powerController;
    private FruitProgressController fruitProgressController;

    [Inject]
    public void Construct(
        PowerController injectedPowerController,
        FruitProgressController injectedFruitProgressController)
    {
        powerController = injectedPowerController
            ?? throw new ArgumentNullException(nameof(injectedPowerController));
        fruitProgressController = injectedFruitProgressController
            ?? throw new ArgumentNullException(nameof(injectedFruitProgressController));
    }

    protected override void OnPickUp(GameObject player)
    {
        if (powerController == null || fruitProgressController == null)
        {
            throw new InvalidOperationException(
                "FruitPickup requires controller injection before collection.");
        }

        powerController.AddPower(powerAmount);
        fruitProgressController.CollectFruit();
    }
}
