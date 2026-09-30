using UnityEngine;
using VContainer;

namespace AdventureIsland.Enemies
{
    public sealed class FireSnakeEnemy : Enemy, IDefeatable, IDestructible
    {
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField, Min(0.01f)] private float fireIntervalSeconds = 1f;
        [SerializeField, Min(0f)] private float projectileSpeed = 4f;
        private float nextFireTime;
        private FireSnakeProjectileProvider projectileProvider;
        private Transform playerTransform;

        public bool TryDefeat() => TryDie();

        public bool TryDestroy() => TryDie();

        [Inject]
        public void Construct(
            FireSnakeProjectileProvider injectedProjectileProvider,
            IPlayerTransformProvider playerTransformProvider)
        {
            projectileProvider = injectedProjectileProvider;
            playerTransform = playerTransformProvider.PlayerTransform;
        }

        private void Update()
        {
            if (!IsAlive ||
                projectileProvider == null ||
                projectileSpawnPoint == null ||
                playerTransform == null ||
                Time.time < nextFireTime)
            {
                return;
            }

            Vector2 directionToPlayer =
                (playerTransform.position - projectileSpawnPoint.position).normalized;
            if (directionToPlayer == Vector2.zero)
            {
                return;
            }

            FireSnakeProjectile projectile = projectileProvider.Get(projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            if (projectile == null || !projectile.Launch(directionToPlayer * projectileSpeed))
            {
                return;
            }

            nextFireTime = Time.time + fireIntervalSeconds;
        }

        protected override void OnRespawned() => nextFireTime = Time.time;
    }
}
