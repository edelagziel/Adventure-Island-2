using System;
using UnityEngine;
using VContainer;

namespace AdventureIsland.Fairy
{
    [DisallowMultipleComponent]
    public sealed class FairyContactHandler : MonoBehaviour
    {
        private IPlayerProtectionState playerProtection;

        [Inject]
        public void Construct(IPlayerProtectionState injectedPlayerProtection)
        {
            playerProtection = injectedPlayerProtection
                ?? throw new ArgumentNullException(nameof(injectedPlayerProtection));
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            TryDestroyContact(collision.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TryDestroyContact(other.gameObject);
        }

        private void TryDestroyContact(GameObject contactObject)
        {
            if (playerProtection == null || !playerProtection.IsActive)
            {
                return;
            }

            foreach (MonoBehaviour behaviour in
                contactObject.GetComponentsInParent<MonoBehaviour>(true))
            {
                if (behaviour is IDestructible destructible)
                {
                    destructible.TryDestroy();
                    return;
                }
            }
        }
    }
}
