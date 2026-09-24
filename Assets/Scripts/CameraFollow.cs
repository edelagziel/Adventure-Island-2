using UnityEngine;

public sealed class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    private float xOffset;

    private void Awake()
    {
        xOffset = transform.position.x - target.position.x;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        transform.position = new Vector3(
            target.position.x + xOffset,
            this.transform.position.y,
            this.transform.position.z
        );
    }
}
