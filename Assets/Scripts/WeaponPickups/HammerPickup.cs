using System;
using AdventureIsland.Combat;
using UnityEngine;
using VContainer;

[DisallowMultipleComponent]
public sealed class HammerPickup : PickUp
{
    private PlayerWeaponCollector collector;
    private HammerWeapon hammerWeapon;

    [Inject]
    public void Construct(
        PlayerWeaponCollector injectedCollector,
        HammerWeapon injectedHammerWeapon)
    {
        collector = injectedCollector
            ?? throw new ArgumentNullException(nameof(injectedCollector));
        hammerWeapon = injectedHammerWeapon
            ?? throw new ArgumentNullException(nameof(injectedHammerWeapon));
    }

    protected override void OnPickUp(GameObject player)
    {
        if (collector == null || hammerWeapon == null)
        {
            throw new InvalidOperationException(
                "HammerPickup requires weapon injection before collection.");
        }

        collector.Collect(hammerWeapon);
    }
}
