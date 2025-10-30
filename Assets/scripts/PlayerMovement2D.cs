using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
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
    public float dashForce = 14f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.6f;
    private bool isDashing = false;
    private float dashCooldownLeft = 0f;
    private int dashDir = 1;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    private bool isGrounded;
    private float moveInput;
    private bool canMove = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // si ton sprite est dans un enfant
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        jumpsLeft = maxJumps;
    }

    void Update()
    {
        // si un dialogue nous bloque
        if (!canMove)
            return;

        // g�rer le cooldown du dash
        if (dashCooldownLeft > 0f)
            dashCooldownLeft -= Time.deltaTime;

        // si on est en train de dasher, on ne lit pas d'input ici
        if (isDashing)
            return;

        // input horizontal
        moveInput = Input.GetAxisRaw("Horizontal");

        // check sol
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

        // saut normal (et double saut)
        if (jumpsLeft > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpsLeft--;
        }
    }

    void TryDash()
    {
        // pas de spam
        if (dashCooldownLeft > 0f)
            return;

        // direction du dash
        if (Mathf.Abs(moveInput) > 0.01f)
            dashDir = moveInput > 0 ? 1 : -1;
        else if (sprite != null)
            dashDir = sprite.flipX ? -1 : 1;
        else
            dashDir = 1;

        isDashing = true;
        dashCooldownLeft = dashCooldown;
        StartCoroutine(DashRoutine());
    }

    System.Collections.IEnumerator DashRoutine()
    {
        float t = dashDuration;
        // on force la vitesse direct
        rb.linearVelocity = new Vector2(dashDir * dashForce, 0f);

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

    // appel� par ton PNJ pour bloquer le joueur
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
