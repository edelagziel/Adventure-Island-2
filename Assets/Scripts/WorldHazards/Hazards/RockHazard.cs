using System;
using UnityEngine;
using VContainer;

namespace AdventureIsland.WorldHazards
{
    [DisallowMultipleComponent]
    public sealed class RockHazard : MonoBehaviour, IBreakableObstacle,
        IDestructible, IStageResettable
    {
        private const int CollisionPowerDamage = 3;

        private IPlayerDamageReceiver playerDamageReceiver;

        [Inject]
        public void Construct(IPlayerDamageReceiver injectedPlayerDamageReceiver)
        {
            playerDamageReceiver = injectedPlayerDamageReceiver
                ?? throw new ArgumentNullException(nameof(injectedPlayerDamageReceiver));
        }

        public bool TryBreak()
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
            return TryBreak();
        }

        public void ResetStageState()
        {
            gameObject.SetActive(true);
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

            IActiveAnimalMount activeAnimalMount = null;
            foreach (MonoBehaviour behaviour in
                other.GetComponentsInParent<MonoBehaviour>(true))
            {
                if (behaviour is IActiveAnimalMount mount)
                {
                    activeAnimalMount = mount;
                    break;
                }
            }

            if (activeAnimalMount != null && activeAnimalMount.HasActiveAnimal)
            {
                activeAnimalMount.ClearActiveAnimal();
                TryBreak();
                return;
            }

            if (playerDamageReceiver == null)
            {
                throw new InvalidOperationException(
                    "RockHazard requires IPlayerDamageReceiver injection before collision handling.");
            }

            playerDamageReceiver.TryTakeDamage(CollisionPowerDamage);
        }
    }
}
