using UnityEngine;

namespace AdventureIsland.Enemies
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class GroundedJumpMotor : MonoBehaviour
    {
        private const float GroundContactIgnoreSeconds = 0.1f;

        private Rigidbody2D body;
        private float lastJumpTime;

        public bool IsJumping { get; private set; }

        private void Awake() => body = GetComponent<Rigidbody2D>();
        public bool TryJump(float horizontalSpeed, float verticalSpeed)
        {
            if (IsJumping) return false;
            body.linearVelocity = new Vector2(horizontalSpeed, verticalSpeed);
            lastJumpTime = Time.time;
            IsJumping = true;
            return true;
        }
        public void ResetJumpState()
        {
            IsJumping = false;
            body.linearVelocity = Vector2.zero;
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!IsJumping ||
                body.linearVelocity.y > 0f ||
                Time.time - lastJumpTime < GroundContactIgnoreSeconds)
            {
                return;
            }

            for (int index = 0; index < collision.contactCount; index++)
            {
                if (collision.GetContact(index).normal.y > 0.5f)
                {
                    IsJumping = false;
                    return;
                }
            }
        }
    }
}
