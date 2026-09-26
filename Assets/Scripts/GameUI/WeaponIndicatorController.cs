using System;
using AdventureIsland.Combat;

public sealed class WeaponIndicatorController : IDisposable
{
    private readonly WeaponController weaponController;
    private readonly IWeaponIndicatorView weaponIndicatorView;

    public WeaponIndicatorController(
        WeaponController weaponController,
        IWeaponIndicatorView weaponIndicatorView)
    {
        this.weaponController = weaponController
            ?? throw new ArgumentNullException(nameof(weaponController));
        this.weaponIndicatorView = weaponIndicatorView
            ?? throw new ArgumentNullException(nameof(weaponIndicatorView));

        weaponController.ActiveWeaponChanged += UpdateView;
        UpdateView();
    }

    public void Dispose()
    {
        weaponController.ActiveWeaponChanged -= UpdateView;
    }

    private void UpdateView()
    {
        weaponIndicatorView.UpdateWeaponDisplay(weaponController.ActiveWeapon);
    }
}
