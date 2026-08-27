using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsView : MonoBehaviour, ICoinView
{
    public TextMeshProUGUI coinsText;
    public void UpdateCoinsAmount(int amount)
    {
        if (coinsText != null)
            coinsText.text = "Coins: " + amount;
    }
}
