using UnityEngine;
using VContainer;

namespace AdventureIsland.Enemies
{
    [RequireComponent(typeof(GroundedJumpMotor))]
    public sealed class FrogEnemy : Enemy
    {
        [SerializeField, Min(0f)] private float triggerDistance = 3f;
        [SerializeField, Min(0f)] private float jumpHorizontalSpeed = 3f;
        [SerializeField, Min(0f)] private float jumpVerticalSpeed = 6f;
        [SerializeField, Min(0.01f)] private float jumpCooldownSeconds = 1f;
        [SerializeField] private bool spriteFacesLocalRight = true;

        private GroundedJumpMotor jumpMotor;
        private Transform playerTransform;
        private float nextJumpTime;

        [Inject]
        public void Construct(IPlayerTransformProvider playerTransformProvider)
        {
            playerTransform = playerTransformProvider.PlayerTransform;
        }

        private void Start()
        {
            jumpMotor = GetComponent<GroundedJumpMotor>();
        }

        private void Update()
        {
            if (!IsAlive || playerTransform == null || Time.time < nextJumpTime ||
                Vector2.Distance(transform.position, playerTransform.position) > triggerDistance)
            {
                return;
            }

            float fixedHorizontalDirection = spriteFacesLocalRight ? 1f : -1f;
            float horizontalOffset = playerTransform.position.x - transform.position.x;
            if (horizontalOffset * fixedHorizontalDirection <= 0f)
            {
                return;
            }

            if (jumpMotor != null &&
                jumpMotor.TryJump(fixedHorizontalDirection * jumpHorizontalSpeed, jumpVerticalSpeed))
            {
                nextJumpTime = Time.time + jumpCooldownSeconds;
            }
        }

        protected override void OnDeathStarted() => jumpMotor?.ResetJumpState();

        protected override void OnRespawned()
        {
            jumpMotor?.ResetJumpState();
            nextJumpTime = Time.time;
        }

    }
}
