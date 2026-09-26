using System.Collections;
using UnityEngine;

namespace AdventureIsland.Fairy
{
    [DisallowMultipleComponent]
    public sealed class FairyProtection : MonoBehaviour, IPlayerProtectionState, IResettable
    {
        [SerializeField, Min(0.01f)] private float durationSeconds = 10f;
        [SerializeField] private GameObject fairyVisual;

        private Coroutine expirationCoroutine;

        public bool IsActive { get; private set; }
        public float RemainingSeconds { get; private set; }

        private void Awake()
        {
            ResetState();
        }

        public void Activate()
        {
            CancelExpiration();

            IsActive = true;
            RemainingSeconds = durationSeconds;

            if (fairyVisual != null)
            {
                fairyVisual.SetActive(true);
            }

            expirationCoroutine = StartCoroutine(ExpireAfterDuration());
        }

        public bool ResetState()
        {
            bool changed = IsActive || RemainingSeconds > 0f;

            CancelExpiration();
            IsActive = false;
            RemainingSeconds = 0f;

            if (fairyVisual != null)
            {
                fairyVisual.SetActive(false);
            }

            return changed;
        }

        private void OnDisable()
        {
            ResetState();
        }

        private IEnumerator ExpireAfterDuration()
        {
            while (RemainingSeconds > 0f)
            {
                RemainingSeconds = Mathf.Max(0f, RemainingSeconds - Time.deltaTime);
                yield return null;
            }

            expirationCoroutine = null;
            IsActive = false;

            if (fairyVisual != null)
            {
                fairyVisual.SetActive(false);
            }
        }

        private void CancelExpiration()
        {
            if (expirationCoroutine == null)
            {
                return;
            }

            StopCoroutine(expirationCoroutine);
            expirationCoroutine = null;
        }
    }
}
