using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarInvinciblePowerUp : IPowerUp
{
    public void ApplyPowerUp(GameObject player)
    {
        if(player != null)
        {
            Debug.Log("Start Star Invincible!");
            PlayerInvincible playerInvincible = player.GetComponent<PlayerInvincible>();
            if (playerInvincible != null)
                playerInvincible.ActivateInvincibility();
        }
    }
}
