using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class GreenAnimal : Animal
{
    [SerializeField] private Collider2D attackHitbox;
    [SerializeField] private SpriteRenderer visual;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite[] attackFrames;
    [SerializeField, Min(0f)] private float attackWindowSeconds = 0.4f;
    [SerializeField, Min(0f)] private float attackCooldownSeconds = 0.5f;

    private Coroutine attackCoroutine;
    private float nextAttackTime;

    private void Awake()
    {
        SetAttackHitboxActive(false);
        ShowIdleVisual();
    }

    private void OnDisable()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        SetAttackHitboxActive(false);
        ShowIdleVisual();
    }

    protected override bool CanAttack()
    {
        return attackHitbox != null &&
            attackCoroutine == null &&
            Time.time >= nextAttackTime &&
            isActiveAndEnabled;
    }

    protected override void PerformAttack()
    {
        SetAttackHitboxActive(true);
        attackCoroutine = StartCoroutine(PlaySpinAttack());
    }

    protected override void StartCooldown()
    {
        nextAttackTime = Time.time + attackCooldownSeconds;
    }

    private IEnumerator PlaySpinAttack()
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

        SetAttackHitboxActive(false);
        ShowIdleVisual();
        attackCoroutine = null;
    }

    private void ShowIdleVisual()
    {
        if (visual != null && idleSprite != null)
        {
            visual.sprite = idleSprite;
        }
    }

    private void SetAttackHitboxActive(bool isActive)
    {
        if (attackHitbox != null)
        {
            attackHitbox.enabled = isActive;
        }
    }
}
