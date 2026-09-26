using AdventureIsland.Presentation;
using UnityEngine;

namespace AdventureIsland.Enemies
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Enemy))]
    [RequireComponent(typeof(GroundedJumpMotor))]
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class EnemyStateSpriteAnimation : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer targetRenderer;
        [SerializeField] private Sprite[] idleFrames;
        [SerializeField] private Sprite[] jumpFrames;
        [SerializeField, Min(0.01f)] private float secondsPerFrame = 0.1f;

        private Enemy enemy;
        private GroundedJumpMotor jumpMotor;
        private SpriteFramePlayer framePlayer;
        private bool wasAlive;
        private bool wasJumping;
        private bool isConfigured;

        private void Awake()
        {
            enemy = GetComponent<Enemy>();
            jumpMotor = GetComponent<GroundedJumpMotor>();

            if (targetRenderer == null)
            {
                targetRenderer = GetComponent<SpriteRenderer>();
            }

            framePlayer = new SpriteFramePlayer();
            isConfigured = enemy != null && jumpMotor != null &&
                HasValidFrames(idleFrames) &&
                HasValidFrames(jumpFrames) &&
                ConfigureFrames(idleFrames);

            if (!isConfigured)
            {
                Debug.LogError(
                    $"{nameof(EnemyStateSpriteAnimation)} on '{name}' requires an Enemy, " +
                    "GroundedJumpMotor, SpriteRenderer, and valid idle and jump frames.",
                    this);
                enabled = false;
            }
        }

        private void Update()
        {
            if (!isConfigured)
            {
                return;
            }

            if (!enemy.IsAlive)
            {
                framePlayer.Stop();
                wasAlive = false;
                return;
            }

            bool isJumping = jumpMotor.IsJumping;
            if (!wasAlive || isJumping != wasJumping)
            {
                wasAlive = true;
                wasJumping = isJumping;
                ConfigureFrames(isJumping ? jumpFrames : idleFrames);
                framePlayer.PlayLoop();
            }

            framePlayer.Tick(Time.deltaTime);
        }

        private bool ConfigureFrames(Sprite[] frames)
        {
            return framePlayer.TryConfigure(
                targetRenderer,
                frames,
                secondsPerFrame);
        }

        private bool HasValidFrames(Sprite[] frames)
        {
            if (targetRenderer == null || frames == null ||
                frames.Length == 0 || secondsPerFrame <= 0f)
            {
                return false;
            }

            foreach (Sprite frame in frames)
            {
                if (frame == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
