using UnityEngine;

namespace AdventureIsland.Combat
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class BoomerangProjectile : Projectile
    {
        [SerializeField, Min(0.01f)] private float outwardDistance = 5f;
        [SerializeField, Min(0.01f)] private float outwardSpeed = 6f;
        [SerializeField, Min(0.01f)] private float arcHeight = 1.5f;
        [SerializeField, Min(0.01f)] private float returnSpeed = 8f;
        [SerializeField, Min(0.01f)] private float returnSteering = 20f;
        [SerializeField, Min(0.01f)] private float rotationSpeed = 720f;
        [SerializeField, Min(0.01f)] private float catchRadius = 0.5f;
        [SerializeField, Min(0.01f)] private float lifetimeSeconds = 5f;

        private Rigidbody2D body;
        private Transform spawnPoint;
        private Transform player;
        private Vector2 flightVelocity;
        private float outwardTravelled;
        private float outwardGravity;
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
            float outwardDuration = outwardDistance / outwardSpeed;
            flightVelocity = new Vector2(
                outwardSpeed * outwardDirection,
                4f * arcHeight / outwardDuration);
            outwardGravity = 8f * arcHeight / (outwardDuration * outwardDuration);
            outwardTravelled = 0f;
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

            body.MoveRotation(body.rotation + rotationSpeed * Time.fixedDeltaTime);

            if (!returning)
            {
                MoveOutward(Time.fixedDeltaTime);
                return;
            }

            Vector2 target = player.position;
            if (Vector2.Distance(body.position, target) <= catchRadius)
            {
                ReleaseToPool();
                return;
            }

            Vector2 desiredVelocity = (target - body.position).normalized * returnSpeed;
            flightVelocity = Vector2.MoveTowards(
                flightVelocity,
                desiredVelocity,
                returnSteering * Time.fixedDeltaTime);

            Vector2 nextPosition = body.position + flightVelocity * Time.fixedDeltaTime;
            if (Vector2.Distance(nextPosition, target) <= catchRadius)
            {
                ReleaseToPool();
                return;
            }

            body.MovePosition(nextPosition);
        }

        private void MoveOutward(float deltaTime)
        {
            float remainingDistance = outwardDistance - outwardTravelled;
            float stepTime = Mathf.Min(deltaTime, remainingDistance / outwardSpeed);
            Vector2 displacement = flightVelocity * stepTime +
                Vector2.down * (0.5f * outwardGravity * stepTime * stepTime);

            flightVelocity.y -= outwardGravity * stepTime;
            outwardTravelled += outwardSpeed * stepTime;
            body.MovePosition(body.position + displacement);

            if (outwardTravelled >= outwardDistance)
            {
                returning = true;
            }
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
            flightVelocity = Vector2.zero;
            outwardTravelled = 0f;
            outwardGravity = 0f;
            outwardDirection = 0f;
            remainingLifetime = 0f;
            hasLaunched = false;
            returning = false;
        }

        private bool HasValidConfiguration()
        {
            return IsFinite(outwardDistance) && outwardDistance > 0f &&
                IsFinite(outwardSpeed) && outwardSpeed > 0f &&
                IsFinite(arcHeight) && arcHeight > 0f &&
                IsFinite(returnSpeed) && returnSpeed > 0f &&
                IsFinite(returnSteering) && returnSteering > 0f &&
                IsFinite(rotationSpeed) && rotationSpeed > 0f &&
                IsFinite(catchRadius) && catchRadius > 0f &&
                IsFinite(lifetimeSeconds) && lifetimeSeconds > 0f;
        }

        private static bool IsFinite(float value)
        {
            return !float.IsNaN(value) && !float.IsInfinity(value);
        }
    }
}
