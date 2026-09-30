using System;
using UnityEngine;
using VContainer;

namespace AdventureIsland.WorldHazards
{
    [DisallowMultipleComponent]
    public sealed class BonfireHazard : MonoBehaviour,
        IDestructible, IStageResettable
    {
        private IPlayerFailureHandler playerFailureHandler;
        private IPlayerProtectionState playerProtection;

        [Inject]
        public void Construct(
            IPlayerFailureHandler injectedPlayerFailureHandler,
            IPlayerProtectionState injectedPlayerProtection)
        {
            playerFailureHandler = injectedPlayerFailureHandler
                ?? throw new ArgumentNullException(nameof(injectedPlayerFailureHandler));
            playerProtection = injectedPlayerProtection
                ?? throw new ArgumentNullException(nameof(injectedPlayerProtection));
        }

        private bool TryExtinguish()
        {
            if (!gameObject.activeSelf)
            {
                return false;
            }

            gameObject.SetActive(false);
            return true;
        }

        public bool TryDestroy()
        {
            return TryExtinguish();
        }

        public void ResetStageState()
        {
            gameObject.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }

            if (playerProtection != null && playerProtection.IsActive)
            {
                TryExtinguish();
                return;
            }

            foreach (MonoBehaviour behaviour in
                other.GetComponentsInParent<MonoBehaviour>(true))
            {
                if (behaviour is IActiveAnimalMount mount &&
                    mount.HasActiveAnimal)
                {
                    mount.ClearActiveAnimal();
                    TryExtinguish();
                    return;
                }
            }

            if (playerFailureHandler == null || playerProtection == null)
            {
                throw new InvalidOperationException(
                    "BonfireHazard requires failure and player protection injection before trigger handling.");
            }

            playerFailureHandler.TryHandlePlayerFailure();
        }
    }
}
