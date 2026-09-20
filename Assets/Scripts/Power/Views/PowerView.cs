using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[DisallowMultipleComponent]
public sealed class PowerView : MonoBehaviour, IPowerView
{
    [SerializeField] private TMP_Text powerText;
    [FormerlySerializedAs("powerFillImage")]
    [SerializeField] private Image powerBarFill;

    public void UpdatePowerDisplay(int currentPower, int maximumPower)
    {
        if (powerText != null)
        {
            powerText.text = $"{currentPower} / {maximumPower}";
        }

        if (powerBarFill != null)
        {
            powerBarFill.fillAmount = (float)currentPower / maximumPower;
        }
    }
}
