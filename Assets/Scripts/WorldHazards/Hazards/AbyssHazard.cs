using System;
using UnityEngine;
using VContainer;

namespace AdventureIsland.WorldHazards
{
    [DisallowMultipleComponent]
    public sealed class AbyssHazard : MonoBehaviour
    {
        private IPlayerFailureHandler playerFailureHandler;

        [Inject]
        public void Construct(IPlayerFailureHandler injectedPlayerFailureHandler)
        {
            playerFailureHandler = injectedPlayerFailureHandler
                ?? throw new ArgumentNullException(nameof(injectedPlayerFailureHandler));
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
                    "AbyssHazard requires IPlayerFailureHandler injection before trigger handling.");
            }

            playerFailureHandler.TryHandlePlayerFailure();
        }
    }
}
