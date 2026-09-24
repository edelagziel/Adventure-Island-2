using UnityEngine;

namespace AdventureIsland.Combat
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class BoomerangProjectile : Projectile
    {
        [SerializeField, Min(0.01f)] private float outwardDistance = 5f;
        [SerializeField, Min(0.01f)] private float outwardSpeed = 6f;
        [SerializeField, Min(0.01f)] private float returnSpeed = 8f;
        [SerializeField, Min(0.01f)] private float catchRadius = 0.5f;
        [SerializeField, Min(0.01f)] private float lifetimeSeconds = 5f;

        private Rigidbody2D body;
        private Transform spawnPoint;
        private Transform player;
        private Vector2 outwardStart;
        private float outwardDirection;
        private float remainingLifetime;
        private Vector3 initialScale;
        private Quaternion initialRotation;
        private bool initialSimulated;
        private bool initialized;
        private bool hasLaunched;
        private bool returning;

        private void Awake()
        {
            EnsureInitialized();
        }

        private void EnsureInitialized()
        {
            if (initialized)
            {
                return;
            }

            body = GetComponent<Rigidbody2D>();
            if (body == null)
            {
                return;
            }

            initialScale = transform.localScale;
            initialRotation = transform.rotation;
            initialSimulated = body.simulated;
            initialized = true;
        }

        internal bool TryConfigure(Transform point)
        {
            if (!IsBoundToPool || hasLaunched || spawnPoint != null ||
                point == null || point.parent == null || !HasValidConfiguration())
            {
                return false;
            }

            EnsureInitialized();
            if (body == null || body.bodyType != RigidbodyType2D.Kinematic)
            {
                return false;
            }

            spawnPoint = point;
            return true;
        }

        public override bool TryLaunch()
        {
            if (!IsBoundToPool || hasLaunched)
            {
                return false;
            }

            if (spawnPoint == null || spawnPoint.parent == null || body == null)
            {
                ReleaseToPool();
                return false;
            }

            Vector3 position = spawnPoint.position;
            float facingScale = spawnPoint.lossyScale.x;
            if (!IsFinite(position.x) || !IsFinite(position.y) ||
                !IsFinite(facingScale) || facingScale == 0f)
            {
                ReleaseToPool();
                return false;
            }

            player = spawnPoint.parent;
            outwardDirection = Mathf.Sign(facingScale);
            outwardStart = position;
            remainingLifetime = lifetimeSeconds;
            returning = false;
            transform.position = position;
            Vector3 scale = initialScale;
            scale.x = Mathf.Abs(scale.x) * outwardDirection;
            transform.localScale = scale;

            gameObject.SetActive(true);
            if (!body.simulated)
            {
                ReleaseToPool();
                return false;
            }

            hasLaunched = true;
            return true;
        }

        private void FixedUpdate()
        {
            if (!hasLaunched)
            {
                return;
            }

            remainingLifetime -= Time.fixedDeltaTime;
            if (remainingLifetime <= 0f || player == null)
            {
                ReleaseToPool();
                return;
            }

            if (!returning)
            {
                float traveled = Mathf.Abs(body.position.x - outwardStart.x);
                float remaining = outwardDistance - traveled;
                if (remaining <= 0f)
                {
                    returning = true;
                }
                else
                {
                    float step = Mathf.Min(outwardSpeed * Time.fixedDeltaTime, remaining);
                    body.MovePosition(body.position + Vector2.right * (outwardDirection * step));
                    if (step >= remaining)
                    {
                        returning = true;
                    }
                    return;
                }
            }

            Vector2 target = player.position;
            if (Vector2.Distance(body.position, target) <= catchRadius)
            {
                ReleaseToPool();
                return;
            }

            body.MovePosition(Vector2.MoveTowards(
                body.position, target, returnSpeed * Time.fixedDeltaTime));
        }

        internal override void ResetForPool()
        {
            EnsureInitialized();
            if (body != null)
            {
                body.linearVelocity = Vector2.zero;
                body.angularVelocity = 0f;
                body.simulated = initialSimulated;
            }

            transform.localScale = initialScale;
            transform.rotation = initialRotation;
            transform.position = Vector3.zero;
            spawnPoint = null;
            player = null;
            outwardStart = Vector2.zero;
            outwardDirection = 0f;
            remainingLifetime = 0f;
            hasLaunched = false;
            returning = false;
        }

        private bool HasValidConfiguration()
        {
            return IsFinite(outwardDistance) && outwardDistance > 0f &&
                IsFinite(outwardSpeed) && outwardSpeed > 0f &&
                IsFinite(returnSpeed) && returnSpeed > 0f &&
                IsFinite(catchRadius) && catchRadius > 0f &&
                IsFinite(lifetimeSeconds) && lifetimeSeconds > 0f;
        }

        private static bool IsFinite(float value)
        {
            return !float.IsNaN(value) && !float.IsInfinity(value);
        }
    }
}
