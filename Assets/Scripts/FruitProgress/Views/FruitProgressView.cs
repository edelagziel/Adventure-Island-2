using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class FruitProgressView : MonoBehaviour, IFruitProgressView
{
    [SerializeField] private TMP_Text fruitProgressText;

    public void UpdateFruitProgress(int currentFruitCount, int fruitThreshold)
    {
        if (fruitProgressText != null)
        {
            fruitProgressText.text = $"{currentFruitCount} / {fruitThreshold}";
        }
    }
}
