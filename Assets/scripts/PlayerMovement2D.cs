using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 6f;

    [Header("Jump")]
    public float jumpForce = 12f;
    public int maxJumps = 1;           // 1 = simple saut, 2 = double saut
    private int jumpsLeft;

    [Header("Ground check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.25f;
    public LayerMask whatIsGround;

    [Header("Wall jump")]
    public Transform wallCheckLeft;
    public Transform wallCheckRight;
    public float wallCheckRadius = 0.25f;
    public LayerMask whatIsWall;
    public float wallJumpForce = 12f;
    public float wallJumpHorizontal = 8f;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    private bool isGrounded;
    private float moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        jumpsLeft = maxJumps;
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (isGrounded)
            jumpsLeft = maxJumps;

        if (Input.GetButtonDown("Jump"))
            TryJump();

        FlipSprite();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void TryJump()
    {
        bool onLeftWall = wallCheckLeft && Physics2D.OverlapCircle(wallCheckLeft.position, wallCheckRadius, whatIsWall);
        bool onRightWall = wallCheckRight && Physics2D.OverlapCircle(wallCheckRight.position, wallCheckRadius, whatIsWall);

        if (onLeftWall)
        {
            rb.linearVelocity = new Vector2(wallJumpHorizontal, wallJumpForce);
            return;
        }
        else if (onRightWall)
        {
            rb.linearVelocity = new Vector2(-wallJumpHorizontal, wallJumpForce);
            return;
        }

        if (jumpsLeft > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpsLeft--;
        }
    }

    void FlipSprite()
    {
        if (sprite == null) return;
        float vx = rb.linearVelocity.x;

        if (vx > 0.05f)
            sprite.flipX = false;
        else if (vx < -0.05f)
            sprite.flipX = true;
    }

    public void SetCanMove(bool canMove)
    {
        enabled = canMove;
        if (!canMove && rb != null)
            rb.linearVelocity = Vector2.zero;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        if (wallCheckLeft != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(wallCheckLeft.position, wallCheckRadius);
        }
        if (wallCheckRight != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(wallCheckRight.position, wallCheckRadius);
        }
    }
}
