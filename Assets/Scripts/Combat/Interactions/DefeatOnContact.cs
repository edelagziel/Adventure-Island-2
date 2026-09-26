using UnityEngine;

namespace AdventureIsland.Combat
{
    [DisallowMultipleComponent]
    public sealed class DefeatOnContact : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            TryDefeat(other.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            TryDefeat(collision.gameObject);
        }

        private static bool TryDefeat(GameObject target)
        {
            if (target == null)
            {
                return false;
            }

            foreach (MonoBehaviour behaviour in
                target.GetComponentsInParent<MonoBehaviour>(true))
            {
                if (behaviour is IDefeatable defeatable)
                {
                    return defeatable.TryDefeat();
                }
            }

            return false;
        }
    }
}
