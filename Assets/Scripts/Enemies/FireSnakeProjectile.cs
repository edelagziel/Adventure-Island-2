using AdventureIsland.Combat;
using UnityEngine;

namespace AdventureIsland.Enemies
{
  [RequireComponent(typeof(Rigidbody2D))]
  [RequireComponent(typeof(SpriteRenderer))]
  public sealed class FireSnakeProjectile : Projectile
  {
    [SerializeField, Min(0.01f)]
    private float lifetimeSeconds = 3f;

    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;
    private float remainingLifetime;
    private bool launched;

    private void Awake()
    {
      body = GetComponent<Rigidbody2D>();
      spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool Launch(Vector2 velocity)
    {
      if (!IsBoundToPool || launched)
      {
        return false;
      }

      remainingLifetime = lifetimeSeconds;
      launched = true;
      gameObject.SetActive(true);
      body.gravityScale = 0f;
      body.linearVelocity = velocity;
      spriteRenderer.flipX = velocity.x < 0f;

      return true;
    }

    public override bool TryLaunch()
    {
      return false;
    }

    private void Update()
    {
      if (!launched)
      {
        return;
      }

      remainingLifetime -= Time.deltaTime;

      if (remainingLifetime <= 0f)
      {
        ReleaseToPool();
      }
    }

    protected override void ResetForPool()
    {
      body.gravityScale = 0f;
      body.linearVelocity = Vector2.zero;
      spriteRenderer.flipX = false;
      remainingLifetime = 0f;
      launched = false;
    }
  }
}
