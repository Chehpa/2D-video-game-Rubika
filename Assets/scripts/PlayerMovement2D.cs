using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("Mouvement")]
    public float moveSpeed = 12f;
    public float acceleration = 20f;
    public float deceleration = 30f;

    [Header("Saut")]
    public float jumpForce = 12f;
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.15f;

    [Header("Double Jump")]
    public int maxAirJumps = 1;
    private int airJumpsUsed = 0;

    [Header("Sol")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.25f;
    public LayerMask whatIsGround;

    [Header("Murs")]
    public Transform wallCheckRight;
    public Transform wallCheckLeft;
    public float wallCheckWidth = 0.35f;
    public float wallCheckHeight = 1.2f;
    public LayerMask whatIsWall;
    public float wallSlideSpeed = 1.5f;
    public float wallJumpForce = 12f;
    public Vector2 wallJumpDir = new Vector2(1f, 1.2f);

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.4f;
    public KeyCode dashKey = KeyCode.LeftShift;

    private bool canDash = true;
    private bool isDashing = false;
    private float dashTimeLeft = 0f;
    private float dashCooldownTimer = 0f;
    private int dashDirection = 1; // 1 droite, -1 gauche

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isTouchingRightWall;
    private bool isTouchingLeftWall;
    private bool isWallSliding;

    private float coyoteCounter;
    private float jumpBufferCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        wallJumpDir.Normalize();
    }

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");

        // ---------- SOL ----------
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (isGrounded && !wasGrounded)
        {
            // on touche sol → on rend le dash à nouveau dispo
            canDash = true;
            airJumpsUsed = 0;
        }

        // ---------- COYOTE / BUFFER ----------
        if (isGrounded) coyoteCounter = coyoteTime;
        else coyoteCounter -= Time.deltaTime;

        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        // ---------- DÉTECTION MURS ----------
        isTouchingRightWall = false;
        isTouchingLeftWall = false;

        if (wallCheckRight != null)
        {
            isTouchingRightWall = Physics2D.OverlapBox(
                wallCheckRight.position,
                new Vector2(wallCheckWidth, wallCheckHeight),
                0f,
                whatIsWall
            );
        }

        if (wallCheckLeft != null)
        {
            isTouchingLeftWall = Physics2D.OverlapBox(
                wallCheckLeft.position,
                new Vector2(wallCheckWidth, wallCheckHeight),
                0f,
                whatIsWall
            );
        }

        // ---------- WALL SLIDE ----------
        isWallSliding = false;
        bool touchingAnyWall = (isTouchingLeftWall || isTouchingRightWall);
        if (!isGrounded && touchingAnyWall && rb.linearVelocity.y < 0f && !isDashing)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
        }

        // ---------- DASH INPUT ----------
        // on ne peut pas dash si on est déjà en dash ou en cooldown
        if (Input.GetKeyDown(dashKey) && canDash && !isDashing)
        {
            // direction: si tu tiens gauche/droite on prend ça, sinon on prend le sens du dernier déplacement
            if (inputX > 0.1f) dashDirection = 1;
            else if (inputX < -0.1f) dashDirection = -1;
            // si toujours 0 → par défaut à droite
            isDashing = true;
            canDash = false;
            dashTimeLeft = dashDuration;
        }

        // ---------- DASH EN COURS ----------
        if (isDashing)
        {
            rb.linearVelocity = new Vector2(dashSpeed * dashDirection, 0f); // on annule la gravité le temps du dash
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0f)
            {
                isDashing = false;
                dashCooldownTimer = dashCooldown;
            }

            // pendant le dash on ne fait PAS le reste
            return;
        }

        // ---------- DASH COOLDOWN ----------
        if (!canDash)
        {
            dashCooldownTimer -= Time.deltaTime;
            if (dashCooldownTimer <= 0f)
            {
                // on ne redonne pas le dash ici si tu n'es pas au sol
                // (on redonne le dash à l'atterrissage plus haut)
            }
        }

        // ---------- SAUT ----------
        if (jumpBufferCounter > 0f)
        {
            bool didJump = false;

            // 1) wall jump (prioritaire)
            if (isWallSliding)
            {
                if (isTouchingRightWall)
                {
                    Vector2 force = new Vector2(-wallJumpDir.x, wallJumpDir.y) * wallJumpForce;
                    rb.linearVelocity = force;
                }
                else if (isTouchingLeftWall)
                {
                    Vector2 force = new Vector2(wallJumpDir.x, wallJumpDir.y) * wallJumpForce;
                    rb.linearVelocity = force;
                }

                // reset air jumps après un wall jump
                airJumpsUsed = 0;
                coyoteCounter = 0f;
                didJump = true;
            }
            // 2) saut normal (sol / coyote)
            else if (coyoteCounter > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                didJump = true;
            }
            // 3) double jump
            else if (!isGrounded && airJumpsUsed < maxAirJumps)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                airJumpsUsed++;
                didJump = true;
            }

            if (didJump)
            {
                jumpBufferCounter = 0f;
            }
        }

        // ---------- MOUVEMENT NORMAL (si pas en dash) ----------
        float targetSpeed = inputX * moveSpeed;
        float accel = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float newX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, accel * Time.deltaTime);
        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        Gizmos.color = Color.blue;
        if (wallCheckRight != null)
        {
            Gizmos.DrawWireCube(wallCheckRight.position, new Vector3(wallCheckWidth, wallCheckHeight, 0));
        }

        Gizmos.color = Color.green;
        if (wallCheckLeft != null)
        {
            Gizmos.DrawWireCube(wallCheckLeft.position, new Vector3(wallCheckWidth, wallCheckHeight, 0));
        }
    }
}
