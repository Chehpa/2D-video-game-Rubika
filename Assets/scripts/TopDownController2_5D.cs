using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class TopDownController2_5D : MonoBehaviour
{
    public enum DirectionMode { Eight, Six }

    [Header("Movement")]
    public DirectionMode directionMode = DirectionMode.Six; // 6 directions par d�faut
    public float moveSpeed = 3.5f;
    public float acceleration = 20f;
    public float deceleration = 30f;

    public Vector2 Facing { get; private set; } = Vector2.down;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 targetVel;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        Vector2 raw = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 dir = Vector2.zero;

        if (raw.sqrMagnitude > 0.01f)
        {
            dir = raw.normalized;

            // "Snap" � 6 directions (angles de 60�)
            if (directionMode == DirectionMode.Six)
            {
                float angle = Mathf.Atan2(dir.y, dir.x);
                float sector = Mathf.Round(angle / (Mathf.PI / 3f));
                float snapped = sector * (Mathf.PI / 3f);
                dir = new Vector2(Mathf.Cos(snapped), Mathf.Sin(snapped)).normalized;
            }
            Facing = dir;
        }

        targetVel = dir * moveSpeed;

        // Tri du joueur : plus bas en Y = au-dessus
        if (sr) sr.sortingOrder = -(int)(transform.position.y * 100f);
    }

    void FixedUpdate()
    {
        Vector2 vel = rb.linearVelocity;
        Vector2 delta = targetVel - vel;
        float rate = (targetVel.sqrMagnitude > 0.01f) ? acceleration : deceleration;
        Vector2 change = Vector2.ClampMagnitude(delta, rate * Time.fixedDeltaTime);
        rb.linearVelocity = vel + change;
    }
}
