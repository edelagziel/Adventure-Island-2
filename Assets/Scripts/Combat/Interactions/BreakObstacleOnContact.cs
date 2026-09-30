using UnityEngine;

namespace AdventureIsland.Combat
{
    [DisallowMultipleComponent]
    public sealed class BreakObstacleOnContact : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            TryDestroy(other.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            TryDestroy(collision.gameObject);
        }

        private static bool TryDestroy(GameObject target)
        {
            if (target == null)
            {
                return false;
            }

            foreach (MonoBehaviour behaviour in
                target.GetComponentsInParent<MonoBehaviour>(true))
            {
                if (behaviour is IBreakableObstacle breakableObstacle)
                {
                    return breakableObstacle.TryBreak();
                }
            }

            return false;
        }
    }
}
