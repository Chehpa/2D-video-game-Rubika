using UnityEngine;

public class CameraClamp2D : MonoBehaviour
{
    public Transform target;
    public float smooth = 10f;
    public Vector2 minBounds;
    public Vector2 maxBounds;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = target.position;
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float clampedX = Mathf.Clamp(targetPos.x, minBounds.x + camWidth, maxBounds.x - camWidth);
        float clampedY = Mathf.Clamp(targetPos.y, minBounds.y + camHeight, maxBounds.y - camHeight);

        Vector3 finalPos = new Vector3(clampedX, clampedY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, finalPos, Time.deltaTime * smooth);
    }
}
