using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class PowerView : MonoBehaviour, IPowerView
{
    [SerializeField] private TMP_Text powerText;

    public void UpdatePowerDisplay(int currentPower, int maximumPower)
    {
        if (powerText != null)
        {
            powerText.text = $"Power: {currentPower}/{maximumPower}";
        }
    }
}
