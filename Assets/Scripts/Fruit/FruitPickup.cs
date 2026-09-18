using System;
using UnityEngine;
using VContainer;

[DisallowMultipleComponent]
public sealed class FruitPickup : PickUp
{
    [SerializeField, Min(1)] private int powerAmount = 1;

    private PowerController powerController;

    [Inject]
    public void Construct(PowerController injectedPowerController)
    {
        powerController = injectedPowerController
            ?? throw new ArgumentNullException(nameof(injectedPowerController));
    }

    protected override void OnPickUp(GameObject player)
    {
        if (powerController == null)
        {
            throw new InvalidOperationException(
                "FruitPickup requires PowerController injection before collection.");
        }

        powerController.AddPower(powerAmount);
    }
}
