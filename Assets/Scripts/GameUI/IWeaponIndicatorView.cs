using CombatWeapon = AdventureIsland.Combat.IWeapon;

public interface IWeaponIndicatorView
{
    void UpdateWeaponDisplay(CombatWeapon activeWeapon);
}
