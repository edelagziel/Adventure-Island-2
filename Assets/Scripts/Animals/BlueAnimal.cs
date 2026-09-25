using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class BlueAnimal : Animal
{
    [SerializeField] private BoxCollider2D tailHitbox;
    [SerializeField] private SpriteRenderer visual;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite[] attackFrames;
    [SerializeField, Min(0f)] private float attackWindowSeconds = 0.1f;
    [SerializeField, Min(0f)] private float attackCooldownSeconds = 0.5f;

    private Coroutine attackWindowCoroutine;
    private float nextAttackTime;

    private void Awake()
    {
        SetTailHitboxActive(false);
        ShowIdleVisual();
    }

    private void OnDisable()
    {
        if (attackWindowCoroutine != null)
        {
            StopCoroutine(attackWindowCoroutine);
            attackWindowCoroutine = null;
        }

        SetTailHitboxActive(false);
        ShowIdleVisual();
    }

    protected override bool CanAttack()
    {
        return tailHitbox != null &&
            attackWindowCoroutine == null &&
            Time.time >= nextAttackTime &&
            isActiveAndEnabled;
    }

    protected override void PerformAttack()
    {
        SetTailHitboxActive(true);
        attackWindowCoroutine = StartCoroutine(CloseAttackWindow());
    }

    protected override void StartCooldown()
    {
        nextAttackTime = Time.time + attackCooldownSeconds;
    }

    private IEnumerator CloseAttackWindow()
    {
        if (visual != null && attackFrames != null && attackFrames.Length > 0)
        {
            float frameSeconds = attackWindowSeconds / attackFrames.Length;

            foreach (Sprite frame in attackFrames)
            {
                if (frame != null)
                {
                    visual.sprite = frame;
                }

                yield return new WaitForSeconds(frameSeconds);
            }
        }
        else
        {
            yield return new WaitForSeconds(attackWindowSeconds);
        }

        SetTailHitboxActive(false);
        ShowIdleVisual();
        attackWindowCoroutine = null;
    }

    private void ShowIdleVisual()
    {
        if (visual != null && idleSprite != null)
        {
            visual.sprite = idleSprite;
        }
    }

    private void SetTailHitboxActive(bool isActive)
    {
        if (tailHitbox != null)
        {
            tailHitbox.enabled = isActive;
        }
    }
}
