using System;
using UnityEngine;
using VContainer;

namespace AdventureIsland.WorldHazards
{
    [DisallowMultipleComponent]
    public sealed class BonfireHazard : MonoBehaviour, IExtinguishableObstacle
    {
        private IPlayerFailureHandler playerFailureHandler;

        [Inject]
        public void Construct(IPlayerFailureHandler injectedPlayerFailureHandler)
        {
            playerFailureHandler = injectedPlayerFailureHandler
                ?? throw new ArgumentNullException(nameof(injectedPlayerFailureHandler));
        }

        public bool TryExtinguish()
        {
            if (!gameObject.activeSelf)
            {
                return false;
            }

            gameObject.SetActive(false);
            return true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }

            if (playerFailureHandler == null)
            {
                throw new InvalidOperationException(
                    "BonfireHazard requires IPlayerFailureHandler injection before trigger handling.");
            }

            playerFailureHandler.TryHandlePlayerFailure();
        }
    }
}
