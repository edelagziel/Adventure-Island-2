using System;
using AdventureIsland.Combat;
using UnityEngine;
using VContainer;

[DisallowMultipleComponent]
public sealed class BoomerangPickup : PickUp
{
    private PlayerWeaponCollector collector;
    private BoomerangWeapon boomerangWeapon;

    [Inject]
    public void Construct(
        PlayerWeaponCollector injectedCollector,
        BoomerangWeapon injectedBoomerangWeapon)
    {
        collector = injectedCollector
            ?? throw new ArgumentNullException(nameof(injectedCollector));
        boomerangWeapon = injectedBoomerangWeapon
            ?? throw new ArgumentNullException(nameof(injectedBoomerangWeapon));
    }

    protected override void OnPickUp(GameObject player)
    {
        if (collector == null || boomerangWeapon == null)
        {
            throw new InvalidOperationException(
                "BoomerangPickup requires weapon injection before collection.");
        }

        collector.Collect(boomerangWeapon);
    }
}
