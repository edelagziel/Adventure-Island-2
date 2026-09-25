using UnityEngine;

namespace AdventureIsland.Enemies
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class EnemySpriteFrameAnimation : MonoBehaviour
    {
        [SerializeField] private Sprite[] animationFrames;
        [SerializeField, Min(0.01f)] private float frameIntervalSeconds = 0.1f;

        private Enemy enemy;
        private SpriteRenderer spriteRenderer;

        private bool isConfigured;
        private bool wasAlive;
        private int frameIndex;
        private float nextFrameTime;

        private void Awake()
        {
            enemy = GetComponent<Enemy>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            isConfigured = HasValidConfiguration();

            if (!isConfigured)
            {
                Debug.LogError(
                    $"{nameof(EnemySpriteFrameAnimation)} on '{name}' " +
                    "requires an Enemy component, a SpriteRenderer, " +
                    "at least one non-null animation frame, " +
                    "and a positive frame interval.",
                    this);

                enabled = false;
                return;
            }

            ResetAnimation();
        }

        private void Update()
        {
            if (!isConfigured)
            {
                return;
            }

            if (!enemy.IsAlive)
            {
                wasAlive = false;
                return;
            }

            if (!wasAlive)
            {
                wasAlive = true;
                ResetAnimation();
                return;
            }

            if (Time.time < nextFrameTime)
            {
                return;
            }

            frameIndex = (frameIndex + 1) % animationFrames.Length;
            spriteRenderer.sprite = animationFrames[frameIndex];
            nextFrameTime = Time.time + frameIntervalSeconds;
        }

        private bool HasValidConfiguration()
        {
            if (enemy == null ||
                spriteRenderer == null ||
                animationFrames == null ||
                animationFrames.Length == 0 ||
                frameIntervalSeconds <= 0f)
            {
                return false;
            }

            foreach (Sprite frame in animationFrames)
            {
                if (frame == null)
                {
                    return false;
                }
            }

            return true;
        }

        private void ResetAnimation()
        {
            frameIndex = 0;
            spriteRenderer.sprite = animationFrames[frameIndex];
            nextFrameTime = Time.time + frameIntervalSeconds;
        }
    }
}