using UnityEngine;

public class CameraBounds2D : MonoBehaviour
{
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (cam == null) return;

        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, minBounds.x + camWidth, maxBounds.x - camWidth);
        pos.y = Mathf.Clamp(pos.y, minBounds.y + camHeight, maxBounds.y - camHeight);

        transform.position = pos;
    }
}
