using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class BlueAnimal : Animal
{
    [SerializeField] private BoxCollider2D tailHitbox;
    [SerializeField, Min(0f)] private float attackWindowSeconds = 0.1f;
    [SerializeField, Min(0f)] private float attackCooldownSeconds = 0.5f;

    private Coroutine attackWindowCoroutine;
    private float nextAttackTime;

    private void Awake()
    {
        SetTailHitboxActive(false);
    }

    private void OnDisable()
    {
        if (attackWindowCoroutine != null)
        {
            StopCoroutine(attackWindowCoroutine);
            attackWindowCoroutine = null;
        }

        SetTailHitboxActive(false);
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
        yield return new WaitForSeconds(attackWindowSeconds);
        SetTailHitboxActive(false);
        attackWindowCoroutine = null;
    }

    private void SetTailHitboxActive(bool isActive)
    {
        if (tailHitbox != null)
        {
            tailHitbox.enabled = isActive;
        }
    }
}
