using UnityEngine;

namespace AdventureIsland.Presentation
{
    public sealed class SpriteFramePlayer
    {
        private SpriteRenderer targetRenderer;
        private Sprite[] frames;
        private float secondsPerFrame;
        private bool loop;
        private bool isPlaying;
        private int currentFrame;
        private float elapsedSeconds;

        public bool TryConfigure(
            SpriteRenderer renderer,
            Sprite[] animationFrames,
            float frameDuration)
        {
            if (renderer == null || animationFrames == null ||
                animationFrames.Length == 0 || frameDuration <= 0f)
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

            targetRenderer = renderer;
            frames = animationFrames;
            secondsPerFrame = frameDuration;
            ResetToFirstFrame();
            return true;
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

        public void Tick(float deltaTime)
        {
            if (!isPlaying)
            {
                return;
            }

            elapsedSeconds += deltaTime;

            while (isPlaying && elapsedSeconds >= secondsPerFrame)
            {
                elapsedSeconds -= secondsPerFrame;
                AdvanceFrame();
            }
        }

        private void Play(bool shouldLoop)
        {
            if (targetRenderer == null || frames == null)
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
            targetRenderer.sprite = frames[currentFrame];
        }
    }
}
