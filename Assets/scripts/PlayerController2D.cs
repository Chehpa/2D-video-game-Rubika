using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 6f;

    [Header("Jump")]
    public float jumpForce = 12f;
    public int maxJumps = 1;
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

    [Header("Dash")]
    public float dashForce = 14f;       // vitesse du dash
    public float dashDuration = 0.15f;  // dur�e en secondes
    public float dashCooldown = 0.6f;   // temps entre 2 dash
    private bool isDashing = false;
    private float dashTimeLeft = 0f;
    private float dashCooldownLeft = 0f;
    private int dashDir = 1;            // 1 droite, -1 gauche

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    private bool isGrounded;
    private float moveInput;
    private bool canMove = true;

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
        if (!canMove)
            return;

        // cooldown du dash
        if (dashCooldownLeft > 0f)
            dashCooldownLeft -= Time.deltaTime;

        // si on est en dash, on laisse FixedUpdate g�rer
        if (isDashing)
            return;

        moveInput = Input.GetAxisRaw("Horizontal");

        // sol
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (isGrounded)
            jumpsLeft = maxJumps;

        // saut
        if (Input.GetButtonDown("Jump"))
            TryJump();

        // dash
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.X))
            TryDash();

        // flip
        FlipSprite();
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (isDashing)
        {
            rb.linearVelocity = new Vector2(dashDir * dashForce, 0f);
            return;
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void TryJump()
    {
        bool onLeftWall = wallCheckLeft && Physics2D.OverlapCircle(wallCheckLeft.position, wallCheckRadius, whatIsWall);
        bool onRightWall = wallCheckRight && Physics2D.OverlapCircle(wallCheckRight.position, wallCheckRadius, whatIsWall);

        // wall jump
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

        // saut normal / double saut
        if (jumpsLeft > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpsLeft--;
        }
    }

    void TryDash()
    {
        if (dashCooldownLeft > 0f)
            return;

        // direction du dash: si on bouge pas, on dash dans le sens du sprite
        if (Mathf.Abs(moveInput) > 0.01f)
            dashDir = moveInput > 0 ? 1 : -1;
        else if (sprite != null)
            dashDir = sprite.flipX ? -1 : 1;
        else
            dashDir = 1;

        isDashing = true;
        dashTimeLeft = dashDuration;
        dashCooldownLeft = dashCooldown;
        // on coupe la vitesse verticale pour �viter de tomber pendant le dash
        rb.linearVelocity = new Vector2(dashDir * dashForce, 0f);
        // si tu veux une invincibilit� pendant le dash, c�est ici
        StartCoroutine(StopDashAfterTime());
    }

    System.Collections.IEnumerator StopDashAfterTime()
    {
        float t = dashDuration;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            yield return null;
        }
        isDashing = false;
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
        this.canMove = canMove;
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            isDashing = false;
        }
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
