using System.Collections;
using UnityEngine;

namespace AdventureIsland.Rewards
{
    [DisallowMultipleComponent]
    public sealed class EggRewardPickupGate : MonoBehaviour
    {
        [SerializeField] private Collider2D pickupCollider;
        [SerializeField, Min(0f)] private float collectionDelaySeconds = 0.5f;

        private Coroutine unlockCoroutine;

        public bool TryBeginCollectionLockout()
        {
            if (pickupCollider == null)
            {
                return false;
            }

            CancelPendingUnlock();
            pickupCollider.enabled = false;
            unlockCoroutine = StartCoroutine(EnableCollectionAfterDelay());
            return true;
        }

        public void ResetGate()
        {
            CancelPendingUnlock();

            if (pickupCollider != null)
            {
                pickupCollider.enabled = false;
            }
        }

        private void OnDisable()
        {
            ResetGate();
        }

        private IEnumerator EnableCollectionAfterDelay()
        {
            yield return new WaitForSeconds(collectionDelaySeconds);

            unlockCoroutine = null;
            if (pickupCollider != null)
            {
                pickupCollider.enabled = true;
            }
        }

        private void CancelPendingUnlock()
        {
            if (unlockCoroutine == null)
            {
                return;
            }

            StopCoroutine(unlockCoroutine);
            unlockCoroutine = null;
        }
    }
}
