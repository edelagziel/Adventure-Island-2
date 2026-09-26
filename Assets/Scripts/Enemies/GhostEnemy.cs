using System;
using UnityEngine;
using VContainer;

namespace AdventureIsland.Enemies
{
    [DisallowMultipleComponent]
    public sealed class GhostEnemy : Enemy
    {
        [SerializeField, Min(0f)] private float chaseSpeed = 2f;

        private Transform player;

        [Inject]
        public void Construct(Transform injectedPlayer)
        {
            player = injectedPlayer
                ?? throw new ArgumentNullException(nameof(injectedPlayer));
        }

        public override bool TryDefeat()
        {
            return false;
        }

        private void Update()
        {
            if (!IsAlive || player == null || chaseSpeed <= 0f)
            {
                return;
            }

            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                chaseSpeed * Time.deltaTime);
        }
    }
}
