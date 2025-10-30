using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TestMove2D : MonoBehaviour
{
    public float speed = 6f;
    Rigidbody2D rb;
    Vector2 input;

    void Awake() { rb = GetComponent<Rigidbody2D>(); }

    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + input * speed * Time.fixedDeltaTime);
    }
}
