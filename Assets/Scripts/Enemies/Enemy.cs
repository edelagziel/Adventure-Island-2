using System.Collections;
using AdventureIsland.Presentation;
using UnityEngine;

namespace AdventureIsland.Enemies
{
    public abstract class Enemy : MonoBehaviour, IDestructible
    {
        [SerializeField, Min(0f)] private float respawnDelaySeconds;
        [SerializeField] private SpriteFrameAnimation spriteAnimation;

        private Vector3 originalSpawnPosition;
        private Quaternion originalSpawnRotation;
        private Coroutine respawnCoroutine;
        private SpriteRenderer enemyRenderer;
        private Collider2D enemyCollider;

        public bool IsAlive { get; private set; }
        public bool IsWaitingToRespawn => respawnCoroutine != null;

        private void Awake()
        {
            originalSpawnPosition = transform.position;
            originalSpawnRotation = transform.rotation;
            enemyRenderer = GetComponent<SpriteRenderer>();
            enemyCollider = GetComponent<Collider2D>();
            IsAlive = true;
        }

        public bool TryDie()
        {
            if (!IsAlive || IsWaitingToRespawn)
            {
                return false;
            }

            IsAlive = false;
            if (spriteAnimation != null)
            {
                spriteAnimation.Stop();
            }

            SetRuntimePresence(false);
            OnDeathStarted();
            respawnCoroutine = StartCoroutine(RespawnAfterDelay());
            return true;
        }

        public bool TryDestroy()
        {
            return TryDie();
        }

        internal void ResetRuntimeState()
        {
            if (respawnCoroutine != null)
            {
                StopCoroutine(respawnCoroutine);
                respawnCoroutine = null;
            }

            transform.SetPositionAndRotation(originalSpawnPosition, originalSpawnRotation);
            IsAlive = true;
            SetRuntimePresence(true);

            if (spriteAnimation != null)
            {
                spriteAnimation.PlayLoop();
            }

            OnRespawned();
        }

        protected virtual void OnDeathStarted()
        {
        }

        protected virtual void OnRespawned()
        {
        }

        private void SetRuntimePresence(bool isPresent)
        {
            if (enemyRenderer != null)
            {
                enemyRenderer.enabled = isPresent;
            }

            if (enemyCollider != null)
            {
                enemyCollider.enabled = isPresent;
            }
        }

        private IEnumerator RespawnAfterDelay()
        {
            yield return new WaitForSeconds(respawnDelaySeconds);

            respawnCoroutine = null;
            ResetRuntimeState();
        }
    }
}
