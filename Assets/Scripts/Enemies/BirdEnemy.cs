using UnityEngine;

namespace AdventureIsland.Enemies
{
    public sealed class BirdEnemy : Enemy, IDefeatable, IDestructible
    {
        [SerializeField, Min(0f)] private float horizontalSpeed;
        [SerializeField, Min(0f)] private float verticalSpeed;
        [SerializeField, Min(0f)] private float verticalRange;

        private Vector3 movementOrigin;
        private bool movingDown = true;

        public bool TryDefeat() => TryDie();

        public bool TryDestroy() => TryDie();

        private void Start()
        {
            movementOrigin = transform.position;
        }

        private void Update()
        {
            if (!IsAlive)
            {
                return;
            }

            transform.Translate(Vector3.left * (horizontalSpeed * Time.deltaTime));
            MoveVertically();
        }

        protected override void OnRespawned()
        {
            movementOrigin = transform.position;
            movingDown = true;
        }

        private void MoveVertically()
        {
            if (verticalSpeed <= 0f || verticalRange <= 0f)
            {
                return;
            }

            float direction = movingDown ? -1f : 1f;
            transform.Translate(Vector3.up * (direction * verticalSpeed * Time.deltaTime));

            float maximumY = movementOrigin.y + verticalRange;
            float minimumY = movementOrigin.y - verticalRange;

            if (transform.position.y <= minimumY)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    minimumY,
                    transform.position.z);
                movingDown = false;
            }
            else if (transform.position.y >= maximumY)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    maximumY,
                    transform.position.z);
                movingDown = true;
            }
        }
    }
}
