using UnityEngine;

namespace AdventureIsland.Presentation
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class SpriteFrameAnimation : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer targetRenderer;
        [SerializeField] private Sprite[] frames;
        [SerializeField, Min(0.01f)] private float secondsPerFrame = 0.1f;
        [SerializeField] private bool playOnEnable = true;
        [SerializeField] private bool loop = true;

        private int currentFrame;
        private float elapsedSeconds;
        private bool isConfigured;
        private bool isPlaying;

        private void Awake()
        {
            if (targetRenderer == null)
            {
                targetRenderer = GetComponent<SpriteRenderer>();
            }

            isConfigured = HasValidConfiguration();

            if (!isConfigured)
            {
                Debug.LogError(
                    $"{nameof(SpriteFrameAnimation)} on '{name}' requires a SpriteRenderer, " +
                    "at least one non-null frame, and a positive frame interval.",
                    this);
                enabled = false;
                return;
            }

            ResetToFirstFrame();
        }

        private void OnEnable()
        {
            if (isConfigured && playOnEnable)
            {
                Play(loop);
            }
        }

        private void Update()
        {
            if (!isPlaying)
            {
                return;
            }

            elapsedSeconds += Time.deltaTime;

            while (isPlaying && elapsedSeconds >= secondsPerFrame)
            {
                elapsedSeconds -= secondsPerFrame;
                AdvanceFrame();
            }
        }

        public void PlayLoop()
        {
            Play(true);
        }

        public void PlayOnce()
        {
            Play(false);
        }

        public void Stop()
        {
            isPlaying = false;
        }

        public void ResetToFirstFrame()
        {
            currentFrame = 0;
            elapsedSeconds = 0f;
            ShowCurrentFrame();
        }

        private void Play(bool shouldLoop)
        {
            if (!isConfigured)
            {
                return;
            }

            loop = shouldLoop;
            isPlaying = true;
            ResetToFirstFrame();
        }

        private void AdvanceFrame()
        {
            if (currentFrame + 1 < frames.Length)
            {
                currentFrame++;
                ShowCurrentFrame();
                return;
            }

            if (loop)
            {
                currentFrame = 0;
                ShowCurrentFrame();
            }
            else
            {
                isPlaying = false;
            }
        }

        private void ShowCurrentFrame()
        {
            if (targetRenderer != null && frames != null && frames.Length > 0)
            {
                targetRenderer.sprite = frames[currentFrame];
            }
        }

        private bool HasValidConfiguration()
        {
            if (targetRenderer == null || frames == null || frames.Length == 0 ||
                secondsPerFrame <= 0f)
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
