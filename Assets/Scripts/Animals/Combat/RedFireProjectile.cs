using AdventureIsland.Combat;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class RedFireProjectile : Projectile
{
    [SerializeField, Min(0f)] private float speed = 6f;
    [SerializeField, Min(0.01f)] private float lifetimeSeconds = 2f;

    private Transform spawnPoint;
    private Vector3 initialScale;
    private Quaternion initialRotation;
    private float direction;
    private float remainingLifetime;
    private bool hasLaunched;

    private void Awake()
    {
        initialScale = transform.localScale;
        initialRotation = transform.rotation;
    }

    internal bool TryConfigure(Transform point)
    {
        if (!IsBoundToPool || hasLaunched || spawnPoint != null || point == null ||
            !IsFinite(speed) || speed < 0f ||
            !IsFinite(lifetimeSeconds) || lifetimeSeconds <= 0f)
        {
            return false;
        }

        spawnPoint = point;
        return true;
    }

    public override bool TryLaunch()
    {
        if (!IsBoundToPool || hasLaunched || spawnPoint == null)
        {
            return false;
        }

        Vector3 spawnPosition = spawnPoint.position;
        float facingScale = spawnPoint.lossyScale.x;
        if (!IsFinite(facingScale) || facingScale == 0f ||
            !IsFinite(spawnPosition.x) || !IsFinite(spawnPosition.y))
        {
            ReleaseToPool();
            return false;
        }

        direction = Mathf.Sign(facingScale);
        transform.position = spawnPosition;
        Vector3 scale = initialScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
        gameObject.SetActive(true);

        remainingLifetime = lifetimeSeconds;
        hasLaunched = true;
        return true;
    }

    private void Update()
    {
        if (!hasLaunched)
        {
            return;
        }

        transform.position += Vector3.right * (direction * speed * Time.deltaTime);
        remainingLifetime -= Time.deltaTime;

        if (remainingLifetime <= 0f)
        {
            ReleaseToPool();
        }
    }

    protected override void ResetForPool()
    {
        transform.position = Vector3.zero;
        transform.localScale = initialScale;
        transform.rotation = initialRotation;
        spawnPoint = null;
        direction = 0f;
        remainingLifetime = 0f;
        hasLaunched = false;
    }

    private static bool IsFinite(float value)
    {
        return !float.IsNaN(value) && !float.IsInfinity(value);
    }
}
