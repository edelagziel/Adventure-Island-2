using UnityEngine;

namespace AdventureIsland.Enemies
{
    public sealed class VerticalSpiderEnemy : Enemy
    {
        [SerializeField, Min(0f)] private float verticalSpeed;
        [SerializeField, Min(0f)] private float verticalRange;

        private Vector3 movementOrigin;
        private bool movingUp = true;

        private void Start()
        {
            movementOrigin = transform.position;
        }

        private void Update()
        {
            if (!IsAlive || verticalSpeed <= 0f || verticalRange <= 0f)
            {
                return;
            }

            float direction = movingUp ? 1f : -1f;
            transform.Translate(Vector3.up * (direction * verticalSpeed * Time.deltaTime));

            float maximumY = movementOrigin.y + verticalRange;
            float minimumY = movementOrigin.y - verticalRange;

            if (transform.position.y >= maximumY)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    maximumY,
                    transform.position.z);
                movingUp = false;
            }
            else if (transform.position.y <= minimumY)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    minimumY,
                    transform.position.z);
                movingUp = true;
            }
        }

        protected override void OnRespawned()
        {
            movementOrigin = transform.position;
            movingUp = true;
        }
    }
}
