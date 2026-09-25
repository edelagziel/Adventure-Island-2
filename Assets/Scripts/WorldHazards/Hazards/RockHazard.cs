using System;
using UnityEngine;
using VContainer;

namespace AdventureIsland.WorldHazards
{
    [DisallowMultipleComponent]
    public sealed class RockHazard : MonoBehaviour, IBreakableObstacle
    {
        private const int CollisionPowerDamage = 3;

        private PowerController powerController;

        [Inject]
        public void Construct(PowerController injectedPowerController)
        {
            powerController = injectedPowerController
                ?? throw new ArgumentNullException(nameof(injectedPowerController));
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

            if (powerController == null)
            {
                throw new InvalidOperationException(
                    "RockHazard requires PowerController injection before collision handling.");
            }

            powerController.ReducePower(CollisionPowerDamage);
        }
    }
}
