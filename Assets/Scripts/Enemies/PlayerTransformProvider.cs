using System;
using UnityEngine;

namespace AdventureIsland.Enemies
{
    public sealed class PlayerTransformProvider : IPlayerTransformProvider
    {
        public PlayerTransformProvider(Transform playerTransform)
        {
            PlayerTransform = playerTransform
                ?? throw new ArgumentNullException(nameof(playerTransform));
        }

        public Transform PlayerTransform { get; }
    }
}
