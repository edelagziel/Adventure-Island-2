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

        private PowerController powerController;
        private IPlayerProtectionState playerProtection;

        [Inject]
        public void Construct(
            PowerController injectedPowerController,
            IPlayerProtectionState injectedPlayerProtection)
        {
            powerController = injectedPowerController
                ?? throw new ArgumentNullException(nameof(injectedPowerController));
            playerProtection = injectedPlayerProtection
                ?? throw new ArgumentNullException(nameof(injectedPlayerProtection));
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

            if (playerProtection != null && playerProtection.IsActive)
            {
                return;
            }

            if (powerController == null || playerProtection == null)
            {
                throw new InvalidOperationException(
                    "RockHazard requires Power and player protection injection before collision handling.");
            }

            powerController.ReducePower(CollisionPowerDamage);
        }
    }
}
