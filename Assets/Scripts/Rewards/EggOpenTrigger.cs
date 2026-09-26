using UnityEngine;

namespace AdventureIsland.Rewards
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public sealed class EggOpenTrigger : MonoBehaviour, IStageResettable
    {
        [SerializeField] private Egg egg;
        [SerializeField] private Collider2D openingCollider;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (egg == null || openingCollider == null ||
                !other.CompareTag("Player"))
            {
                return;
            }

            if (egg.TryOpen())
            {
                openingCollider.enabled = false;
            }
        }

        public void ResetStageState()
        {
            if (openingCollider != null)
            {
                openingCollider.enabled = true;
            }
        }
    }
}
