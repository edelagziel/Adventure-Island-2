using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class LivesView : MonoBehaviour, ILivesView
{
    [SerializeField] private TMP_Text livesText;

    public void UpdateLivesDisplay(int currentLives)
    {
        if (livesText != null)
        {
            livesText.text = $"x {currentLives}";
        }
    }
}
