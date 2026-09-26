using UnityEngine;

namespace AdventureIsland.WorldHazards
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class BonfireFlameAnimation : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer targetRenderer;
        [SerializeField] private Sprite[] frames;
        [SerializeField, Min(0.01f)] private float secondsPerFrame = 0.12f;

        private int currentFrame;
        private float elapsedSeconds;

        private void OnEnable()
        {
            currentFrame = 0;
            elapsedSeconds = 0f;
            ShowCurrentFrame();
        }

        private void Update()
        {
            if (targetRenderer == null || frames == null || frames.Length == 0)
            {
                return;
            }

            elapsedSeconds += Time.deltaTime;

            while (elapsedSeconds >= secondsPerFrame)
            {
                elapsedSeconds -= secondsPerFrame;
                currentFrame = (currentFrame + 1) % frames.Length;
                ShowCurrentFrame();
            }
        }

        private void ShowCurrentFrame()
        {
            if (targetRenderer != null && frames != null && frames.Length > 0)
            {
                targetRenderer.sprite = frames[currentFrame];
            }
        }
    }
}
