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

        private bool isConfigured;
        private SpriteFramePlayer framePlayer;

        private void Awake()
        {
            if (targetRenderer == null)
            {
                targetRenderer = GetComponent<SpriteRenderer>();
            }

            framePlayer = new SpriteFramePlayer();
            isConfigured = framePlayer.TryConfigure(
                targetRenderer,
                frames,
                secondsPerFrame);

            if (!isConfigured)
            {
                Debug.LogError(
                    $"{nameof(SpriteFrameAnimation)} on '{name}' requires a SpriteRenderer, " +
                    "at least one non-null frame, and a positive frame interval.",
                    this);
                enabled = false;
                return;
            }

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
            framePlayer.Tick(Time.deltaTime);
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
            framePlayer?.Stop();
        }

        public void ResetToFirstFrame()
        {
            framePlayer?.ResetToFirstFrame();
        }

        private void Play(bool shouldLoop)
        {
            if (!isConfigured)
            {
                return;
            }

            if (shouldLoop)
            {
                framePlayer.PlayLoop();
            }
            else
            {
                framePlayer.PlayOnce();
            }
        }
    }
}
