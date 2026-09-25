using UnityEngine;

namespace AdventureIsland.Combat
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class HammerProjectile : Projectile
    {
        [SerializeField, Min(0f)] private float horizontalSpeed = 5f;
        [SerializeField, Min(0f)] private float initialVerticalSpeed = 5f;
        [SerializeField] private float angularSpeed = 540f;
        [SerializeField, Min(0f)] private float gravityScale = 1f;
        [SerializeField, Min(0.01f)] private float lifetimeSeconds = 5f;

        private Rigidbody2D body;
        private Transform spawnPoint;
        private float remainingLifetime;
        private Vector3 initialScale;
        private Quaternion initialRotation;
        private float initialGravityScale;
        private bool initialSimulated;
        private bool hasLaunched;
        private bool initialized;

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
            initialGravityScale = body.gravityScale;
            initialSimulated = body.simulated;
            initialized = true;
        }

        internal bool TryConfigure(Transform point)
        {
            if (!IsBoundToPool || hasLaunched || spawnPoint != null ||
                point == null || !HasValidMovementConfiguration() ||
                lifetimeSeconds <= 0f || !IsFinite(lifetimeSeconds))
            {
                return false;
            }

            EnsureInitialized();
            if (body == null)
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

            if (spawnPoint == null || body == null)
            {
                ReleaseToPool();
                return false;
            }

            Vector3 spawnPosition = spawnPoint.position;
            float facingScale = spawnPoint.lossyScale.x;
            if (facingScale == 0f || float.IsNaN(facingScale) ||
                float.IsInfinity(facingScale) ||
                float.IsNaN(spawnPosition.x) || float.IsInfinity(spawnPosition.x) ||
                float.IsNaN(spawnPosition.y) || float.IsInfinity(spawnPosition.y))
            {
                ReleaseToPool();
                return false;
            }

            float direction = Mathf.Sign(facingScale);
            transform.position = spawnPosition;
            Vector3 scale = initialScale;
            scale.x = Mathf.Abs(scale.x) * direction;
            transform.localScale = scale;

            gameObject.SetActive(true);
            if (!body.simulated)
            {
                ReleaseToPool();
                return false;
            }

            body.gravityScale = gravityScale;
            body.linearVelocity = new Vector2(
                horizontalSpeed * direction,
                initialVerticalSpeed);
            body.angularVelocity = -angularSpeed * direction;

            remainingLifetime = lifetimeSeconds;
            hasLaunched = true;
            return true;
        }

        private void Update()
        {
            if (!hasLaunched)
            {
                return;
            }

            remainingLifetime -= Time.deltaTime;
            if (remainingLifetime <= 0f)
            {
                ReleaseToPool();
            }
        }

        protected internal override void ResetForPool()
        {
            EnsureInitialized();
            if (body != null)
            {
                body.linearVelocity = Vector2.zero;
                body.angularVelocity = 0f;
                body.gravityScale = initialGravityScale;
                body.simulated = initialSimulated;
            }

            transform.localScale = initialScale;
            transform.rotation = initialRotation;
            transform.position = Vector3.zero;
            remainingLifetime = 0f;
            spawnPoint = null;
            hasLaunched = false;
        }

        private bool HasValidMovementConfiguration()
        {
            return IsFinite(horizontalSpeed) && horizontalSpeed >= 0f &&
                IsFinite(initialVerticalSpeed) && initialVerticalSpeed >= 0f &&
                IsFinite(angularSpeed) &&
                IsFinite(gravityScale) && gravityScale >= 0f;
        }

        private static bool IsFinite(float value)
        {
            return !float.IsNaN(value) && !float.IsInfinity(value);
        }
    }
}
