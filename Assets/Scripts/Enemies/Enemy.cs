using System.Collections;
using UnityEngine;

namespace AdventureIsland.Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float respawnDelaySeconds;

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
            OnDeathStarted();
            respawnCoroutine = StartCoroutine(RespawnAfterDelay());
            return true;
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

            transform.SetPositionAndRotation(originalSpawnPosition, originalSpawnRotation);
            IsAlive = true;
            respawnCoroutine = null;
            OnRespawned();
        }
    }
}
