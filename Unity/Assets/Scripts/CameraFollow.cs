using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 2f;
    public float yOffset = 2f; // keep camera slightly ahead of the player

    private float fixedX;
    private float fixedZ;

    void Start()
    {
        if (target != null)
        {
            fixedX = transform.position.x;
            fixedZ = transform.position.z;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3(
        fixedX,
        target.position.y + yOffset,
        fixedZ
        );

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
}
