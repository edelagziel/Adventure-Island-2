using System;
using UnityEngine;
using VContainer;

namespace AdventureIsland.Enemies
{
    [DisallowMultipleComponent]
    public sealed class EnemyContactDamage : MonoBehaviour
    {
        [SerializeField, Min(1)] private int damageAmount = 1;

        private IPlayerDamageReceiver playerDamageReceiver;

        [Inject]
        public void Construct(IPlayerDamageReceiver injectedPlayerDamageReceiver)
        {
            playerDamageReceiver = injectedPlayerDamageReceiver
                ?? throw new ArgumentNullException(nameof(injectedPlayerDamageReceiver));
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            HandleCollision(collision.gameObject);
        }

        private void HandleCollision(GameObject other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            if (playerDamageReceiver == null)
            {
                throw new InvalidOperationException(
                    "EnemyContactDamage requires IPlayerDamageReceiver injection before collision handling.");
            }

            playerDamageReceiver.TryTakeDamage(damageAmount);
        }
    }
}
