using System;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerResetter : MonoBehaviour, IPlayerResetter
{
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D playerRigidbody;

    private void Awake()
    {
        ValidateConfiguration();
    }

    public void ResetToSpawn(Transform spawnPoint)
    {
        if (spawnPoint == null)
        {
            throw new ArgumentNullException(nameof(spawnPoint));
        }

        playerRigidbody.linearVelocity = Vector2.zero;
        playerRigidbody.angularVelocity = 0f;
        playerRigidbody.position = spawnPoint.position;
        player.position = spawnPoint.position;
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
