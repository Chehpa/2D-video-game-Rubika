using UnityEngine;

public class EnemyPatrol2D : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public float arriveDistance = 0.1f;
    public bool flipSprite = true;

    private Transform _target;

    void Start()
    {
        _target = pointB;
    }

    void Update()
    {
        if (_target == null) return;

        // move
        transform.position = Vector3.MoveTowards(transform.position, _target.position, speed * Time.deltaTime);

        // check arrive
        float dist = Vector3.Distance(transform.position, _target.position);
        if (dist <= arriveDistance)
        {
            // swap
            _target = _target == pointA ? pointB : pointA;
            if (flipSprite)
            {
                Vector3 s = transform.localScale;
                s.x *= -1f;
                transform.localScale = s;
            }
        }
    }
}
