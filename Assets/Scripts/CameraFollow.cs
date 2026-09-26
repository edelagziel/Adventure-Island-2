using UnityEngine;

public sealed class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private GameObject stage2Root;

    private float xOffset;
    private float stage1CameraY;

    private void Awake()
    {
        if (target == null)
        {
            return;
        }

        xOffset = transform.position.x - target.position.x;
        stage1CameraY = transform.position.y;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        float cameraY = stage2Root != null && stage2Root.activeInHierarchy
            ? target.position.y
            : stage1CameraY;

        transform.position = new Vector3(target.position.x + xOffset, cameraY, transform.position.z);
    }
}
