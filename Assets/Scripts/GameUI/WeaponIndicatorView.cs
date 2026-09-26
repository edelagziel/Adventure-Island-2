using CombatWeapon = AdventureIsland.Combat.IWeapon;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class WeaponIndicatorView : MonoBehaviour, IWeaponIndicatorView
{
    [SerializeField] private TMP_Text weaponText;

    public void UpdateWeaponDisplay(CombatWeapon activeWeapon)
    {
        if (weaponText == null)
        {
            return;
        }

        weaponText.text = activeWeapon == null
            ? "Weapon: None"
            : $"Weapon: {FormatDisplayName(activeWeapon.GetType().Name, "Weapon")}";
    }

    private static string FormatDisplayName(string typeName, string suffix)
    {
        return typeName.EndsWith(suffix)
            ? typeName.Substring(0, typeName.Length - suffix.Length)
            : typeName;
    }
}
