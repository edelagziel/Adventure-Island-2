using System.Collections;
using AdventureIsland.Presentation;
using UnityEngine;

namespace AdventureIsland.Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float respawnDelaySeconds;
        [SerializeField] private SpriteFrameAnimation spriteAnimation;

        private Vector3 originalSpawnPosition;
        private Quaternion originalSpawnRotation;
        private Coroutine respawnCoroutine;

        public bool IsAlive { get; private set; }
        public bool IsWaitingToRespawn => respawnCoroutine != null;

        private void Awake()
        {
            originalSpawnPosition = transform.position;
            originalSpawnRotation = transform.rotation;
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

            OnDeathStarted();
            respawnCoroutine = StartCoroutine(RespawnAfterDelay());
            return true;
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

        private IEnumerator RespawnAfterDelay()
        {
            yield return new WaitForSeconds(respawnDelaySeconds);

            respawnCoroutine = null;
            ResetRuntimeState();
        }
    }
}
