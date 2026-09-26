using System;
using UnityEngine;
using VContainer;

namespace AdventureIsland.Enemies
{
    [DisallowMultipleComponent]
    public sealed class GhostEnemy : Enemy
    {
        [SerializeField, Min(0f)] private float chaseSpeed = 2f;
        [SerializeField] private bool spriteFacesLocalRight = true;

        private Transform player;
        private SpriteRenderer spriteRenderer;

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

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (!IsAlive || player == null || chaseSpeed <= 0f)
            {
                return;
            }

            FacePlayer();

            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                chaseSpeed * Time.deltaTime);
        }

        private void FacePlayer()
        {
            if (spriteRenderer == null)
            {
                return;
            }

            float horizontalOffset = player.position.x - transform.position.x;
            if (Mathf.Approximately(horizontalOffset, 0f))
            {
                return;
            }

            spriteRenderer.flipX = spriteFacesLocalRight
                ? horizontalOffset < 0f
                : horizontalOffset > 0f;
        }
    }
}
