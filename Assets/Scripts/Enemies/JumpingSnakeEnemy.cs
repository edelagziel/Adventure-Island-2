using UnityEngine;

namespace AdventureIsland.Enemies
{
    [RequireComponent(typeof(GroundedJumpMotor))]
    public sealed class JumpingSnakeEnemy : Enemy, IDefeatable, IDestructible
    {
        [SerializeField, Min(0f)] private float jumpHorizontalSpeed = 2f;
        [SerializeField, Min(0f)] private float jumpVerticalSpeed = 4f;
        [SerializeField, Min(0.01f)] private float jumpIntervalSeconds = 1f;
        [SerializeField] private bool spriteFacesLocalRight;
        private GroundedJumpMotor jumpMotor;
        private float nextJumpTime;

        public bool TryDefeat() => TryDie();

        public bool TryDestroy() => TryDie();

        private void Start()
        {
            jumpMotor = GetComponent<GroundedJumpMotor>();
        }

        private void Update()
        {
            if (!IsAlive ||
                Time.time < nextJumpTime ||
                jumpMotor == null)
            {
                return;
            }

            float transformFacingDirection = transform.right.x >= 0f ? 1f : -1f;
            float spriteFacingDirection = spriteFacesLocalRight ? 1f : -1f;
            if (jumpMotor.TryJump(
                transformFacingDirection * spriteFacingDirection * jumpHorizontalSpeed,
                jumpVerticalSpeed))
            {
                nextJumpTime = Time.time + jumpIntervalSeconds;
            }
        }

        protected override void OnDeathStarted()
        {
            jumpMotor?.ResetJumpState();
        }

        protected override void OnRespawned()
        {
            jumpMotor?.ResetJumpState();
            nextJumpTime = Time.time;
        }
    }
}
