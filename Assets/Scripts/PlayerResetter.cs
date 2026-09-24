using System;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerResetter : MonoBehaviour, IPlayerResetter
{
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D playerRigidbody;

    private Vector3 initialPlayerPosition;

    private void Awake()
    {
        ValidateConfiguration();
        initialPlayerPosition = player.position;
    }

    public void ResetToInitialSpawn()
    {
        playerRigidbody.linearVelocity = Vector2.zero;
        playerRigidbody.angularVelocity = 0f;
        playerRigidbody.position = initialPlayerPosition;
        player.position = initialPlayerPosition;
    }

    private void ValidateConfiguration()
    {
        if (player == null)
        {
            throw new InvalidOperationException("PlayerResetter requires a Player Transform reference.");
        }

        if (playerRigidbody == null)
        {
            throw new InvalidOperationException("PlayerResetter requires a Player Rigidbody2D reference.");
        }
    }
}
