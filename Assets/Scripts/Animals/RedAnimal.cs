using System;
using System.Collections;
using AdventureIsland.Combat;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class RedAnimal : Animal
{
    [SerializeField] private SpriteRenderer visual;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite[] attackFrames;
    [SerializeField] private Transform fireSpawnPoint;
    [SerializeField, Min(0f)] private float attackDurationSeconds = 0.4f;
    [SerializeField, Min(0f)] private float attackCooldownSeconds = 0.5f;

    private ProjectileProvider<RedFireProjectileDirector> projectileProvider;
    private Coroutine attackCoroutine;
    private float nextAttackTime;

    private void Awake()
    {
        ShowIdleVisual();
    }

    private void OnDisable()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        ShowIdleVisual();
    }

    internal void Configure(
        ProjectileProvider<RedFireProjectileDirector> provider)
    {
        projectileProvider = provider
            ?? throw new ArgumentNullException(nameof(provider));
    }

    protected override bool CanAttack()
    {
        return projectileProvider != null &&
            fireSpawnPoint != null &&
            attackCoroutine == null &&
            Time.time >= nextAttackTime &&
            isActiveAndEnabled;
    }

    protected override void PerformAttack()
    {
        Projectile projectile = projectileProvider.GetReady(fireSpawnPoint);
        if (projectile == null || !projectile.TryLaunch())
        {
            return;
        }

        attackCoroutine = StartCoroutine(PlayAttackVisual());
    }

    protected override void StartCooldown()
    {
        nextAttackTime = Time.time + attackCooldownSeconds;
    }

    private IEnumerator PlayAttackVisual()
    {
        if (visual != null && attackFrames != null && attackFrames.Length > 0)
        {
            float frameSeconds = attackDurationSeconds / attackFrames.Length;

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
            yield return new WaitForSeconds(attackDurationSeconds);
        }

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
}
