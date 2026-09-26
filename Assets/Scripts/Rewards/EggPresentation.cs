using UnityEngine;

namespace AdventureIsland.Rewards
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class EggPresentation : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer eggRenderer;
        [SerializeField] private Sprite closedSprite;
        [SerializeField] private Sprite openedSprite;

        private void Awake()
        {
            ShowClosed();
        }

        public void ShowClosed()
        {
            if (eggRenderer != null && closedSprite != null)
            {
                eggRenderer.sprite = closedSprite;
            }
        }

        public void ShowOpened()
        {
            if (eggRenderer != null && openedSprite != null)
            {
                eggRenderer.sprite = openedSprite;
            }
        }
    }
}
