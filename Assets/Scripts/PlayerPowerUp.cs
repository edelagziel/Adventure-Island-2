using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
    public void CollectPowerUp(IPowerUp powerUp)
    {
        powerUp.ApplyPowerUp(this.gameObject);
    }
}
