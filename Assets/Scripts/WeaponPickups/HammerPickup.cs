using System;
using AdventureIsland.Combat;
using UnityEngine;
using VContainer;

[DisallowMultipleComponent]
public sealed class HammerPickup : PickUp
{
    private WeaponController weaponController;
    private HammerWeapon hammerWeapon;

    [Inject]
    public void Construct(
        WeaponController injectedWeaponController,
        HammerWeapon injectedHammerWeapon)
    {
        weaponController = injectedWeaponController
            ?? throw new ArgumentNullException(nameof(injectedWeaponController));
        hammerWeapon = injectedHammerWeapon
            ?? throw new ArgumentNullException(nameof(injectedHammerWeapon));
    }

    protected override void OnPickUp(GameObject player)
    {
        if (weaponController == null || hammerWeapon == null)
        {
            throw new InvalidOperationException(
                "HammerPickup requires weapon injection before collection.");
        }

        hammerWeapon.CollectHammer();
        weaponController.Equip(hammerWeapon);
    }
}
