using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerMovement : MonoBehaviour
{
    [Header("Condition Checks")]
    [SerializeField] private Transform GroundCheckLeft;
    [SerializeField] private Transform GroundCheckRight;
    [SerializeField] private Transform WallCheckTop;
    //[SerializeField] private Transform WallCheckBottom;
    [SerializeField] private LayerMask WhatIsGround;

    // Variables having to do with horizontal movement
    private bool FacingRight = true;
    private float playerHorizontalInput;
    [Header("Horizontal Settings")]
    [SerializeField] private float runSpeed = 40;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] private float velPower;
    [SerializeField] private float frictionAmount = .2f;
    [SerializeField] private float movementBuffer = .2f;

    //Variable having to do with the jump movement
    public bool isGrounded;
    public bool lastIsGrounded;
    private float lastJumpTime;
    private float lastJumpInput = -3;
    public bool isJumping;
    private bool jumpInputReleased;

    [Header("Jumping Settings")]
    [SerializeField] public int extraJumps = 1;
    [SerializeField] private int maxJumps;
    [SerializeField] private float jumpForce;
    [SerializeField] private float stopJumpForce;
    [SerializeField] private float gravityValue = 3f;
    [SerializeField] private float downGravityValue = 5f;
    [SerializeField] private float jumpGraceTime = .05f;
    [SerializeField] private float coyoteTime = .05f;
    [SerializeField] private float jumpTime = .2f;


    // Variables having to do with wall
    public bool onWall = false;
    private bool lastOnWall;
    public bool isSliding;
    public bool isWallJumping;
    private float wallJumpingDir;
    [Header("Wall Settings")]
    [SerializeField] private float slideSpeed;
    [SerializeField] private float wallJumpingDuration = 0.15f;
    [SerializeField] private float wallJumpForce = 10f;
    [SerializeField] private float wallJumpGraceTime = .05f;


    [Header("Dash Settings")]
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashTime;
    [SerializeField] private float downDashForce;
    [SerializeField] private float dashJumpGraceTime = 0.5f;
    private bool canDash = true;
    private bool isDashing;


    [Header("Light Settings")]
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D lightSource;
    [SerializeField] public UnityEngine.Rendering.Universal.Light2D globalLightSource;
    [SerializeField] private float lightMin = 2f;
    [SerializeField] private float lightMax = 5f;
    [SerializeField] private float lightRate = .2f;

    [Header("Health Settings")]
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth = 10;
    [SerializeField] private int healthRate = 5;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Idle Settings")]
    [SerializeField] private int maxTimeIdleBeforeLosingHealth = 50;
    [SerializeField] private float idleEpsilon = 3f;
    private int timeIdleCount = 0;

    private float CheckRadius = .2f;


    //Components
    private Rigidbody2D rb;

    //Time Variables
    private float lastGroundedTime = 0f;
    private float lastWalledTime = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBar.SetHealth(currentHealth, maxHealth);

        
    }

    // Update is called once per frame
    void Update()
    {
        // checking for player state
        isPlayerGrounded();
        //isOnWall();
        //isWallSliding();
        SetAnimation();

        // Checking for user input
        if (!isWallJumping && !isDashing)
        {
            InputManager();
        }
        DefaultInputManager();
        // Setting up animation variables

        //JumpDynamics();

    }

    private void FixedUpdate()
    {
        if (isDashing) { return; }

        if (!isWallJumping)
        {
            Run(playerHorizontalInput * Time.fixedDeltaTime);
        }
        if (!isWallJumping && !isDashing)
        {
            Flip();
        }

        SetLight();
        SetHealth();
        SetTimeIdle();

    }

    private void InputManager()
    {
        playerHorizontalInput = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            StartCoroutine(Jump());
        }

    }

    private void SetLight()
    {
        if (Mathf.Abs(rb.velocityX) > idleEpsilon)
        {
            lightSource.pointLightOuterRadius = Mathf.Clamp(lightSource.pointLightOuterRadius + lightRate, lightMin, lightMax);
        }
        else
        {
            if (timeIdleCount >= maxTimeIdleBeforeLosingHealth)
            {
                lightSource.pointLightOuterRadius = Mathf.Clamp(lightSource.pointLightOuterRadius - lightRate, lightMin, lightMax);
            }
        }
    }

    private void SetHealth()
    {

        if (Mathf.Abs(rb.velocityX) > idleEpsilon)
        {
            if(currentHealth < maxHealth)
            {
                currentHealth += healthRate;
            }
        }
        else
        {
            if((currentHealth > 0) && (timeIdleCount >= maxTimeIdleBeforeLosingHealth))
            {
                currentHealth -= healthRate;
            }
        }
        healthBar.SetHealth(Mathf.Clamp(currentHealth, 0, maxHealth), maxHealth);
    }

    private void SetTimeIdle()
    {
        if (Mathf.Abs(rb.velocityX) > idleEpsilon)
        {
            timeIdleCount = 0;
        }
        else
        {
            timeIdleCount++;
        }
    }

    private void DefaultInputManager()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            lastJumpInput = Time.time;
        }
        if (!isGrounded && Input.GetButtonUp("Jump"))
        {
            stopJump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }


    }

    private void Run(float move)
    {
        // Getting the directions and the top speed we want
        if (Mathf.Abs(move) > movementBuffer)
        {

            float targetSpeed = move * runSpeed;
            //Find the difference between our current speed and the disired speed
            float speedDif = targetSpeed - rb.velocity.x;
            //Chang acceleration depending on situation
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;

            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

            rb.AddForce(movement * Vector2.right);
        }

        if (lastGroundedTime > 0 && Mathf.Abs(move) < movementBuffer)
        {
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(rb.velocity.x);
            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);

        }
    }

    private IEnumerator Jump()
    {
        if (onWall && !isGrounded && Mathf.Abs(playerHorizontalInput) > 0f)
        {
            rb.gravityScale = gravityValue;
            WallJump();
        }
        else if (isGrounded || Time.time - lastGroundedTime < coyoteTime)
        {
            rb.gravityScale = gravityValue;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        else if (!isGrounded && extraJumps > 0)
        {
            extraJumps--;
            rb.gravityScale = gravityValue;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        lastGroundedTime = 0f;
        lastJumpTime = Time.time;
        isJumping = true;
        //canJump = false;
        yield return new WaitForSeconds(jumpTime);
        //canJump = true;


    }
    public void WallJump()
    {
        isWallJumping = true;
        isSliding = false;
        isJumping = true;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(new Vector2(wallJumpingDir * wallJumpForce, jumpForce), ForceMode2D.Impulse);
        float originalScaleX = transform.localScale.x;
        if (originalScaleX != wallJumpingDir)
        {
            Flip();
        }

        Invoke(nameof(StopWallJumping), wallJumpingDuration);
    }

    public IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.gravityScale = 0f;
        if (Input.GetAxisRaw("Vertical") < -0.5f && !isGrounded)
        {
            rb.velocity = new Vector2(0.0f, -downDashForce);
        }
        else
        {
            rb.velocity = new Vector2(transform.localScale.x * dashForce, 0f);
        }
        yield return new WaitForSeconds(dashTime);
        if (Time.time - lastJumpInput < dashJumpGraceTime && extraJumps > 0 && isGrounded)// && canJump)
        {
            StartCoroutine(Jump());
        }
        rb.gravityScale = gravityValue;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;


    }


    private void stopJump()
    {
        if (rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * stopJumpForce);
        }

    }

    private void IncreaseFall()
    {
        if (!isGrounded && rb.velocity.y < 0f && !isSliding)
        {
            rb.gravityScale = Mathf.Clamp(rb.gravityScale * 1.1f, 0f, downGravityValue);
        }
    }
    private void JumpDynamics()
    {
        IncreaseFall();
    }

    private void isPlayerGrounded()
    {

        isGrounded = Physics2D.OverlapCircle(GroundCheckRight.position, CheckRadius, WhatIsGround)
            && Physics2D.OverlapCircle(GroundCheckLeft.position, CheckRadius, WhatIsGround);

        if (isGrounded && !lastIsGrounded)
        {
            extraJumps = maxJumps;
            isJumping = false;
            rb.gravityScale = gravityValue;

        }

        if (isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        lastIsGrounded = isGrounded;

        if (Time.time - lastJumpInput < jumpGraceTime && extraJumps > 0 && isGrounded)
        {
            isJumping = true;
            StartCoroutine(Jump());
        }
    }

    private void isOnWall()
    {
        onWall = Physics2D.OverlapCircle(WallCheckTop.position, CheckRadius, WhatIsGround);

        if (onWall && !lastOnWall)
        {
            isWallJumping = false;
            extraJumps = maxJumps;
            isJumping = false;
            rb.gravityScale = gravityValue;
            CancelInvoke(nameof(StopWallJumping));

        }

        lastOnWall = onWall;

        if (onWall)
        {
            lastWalledTime = Time.time;
        }

        if (Time.time - lastJumpInput < wallJumpGraceTime && extraJumps > 0 && onWall)
        {
            StartCoroutine(Jump());
        }
    }

    private void isWallSliding()
    {
        isSliding = (!isGrounded && onWall && Mathf.Abs(playerHorizontalInput) > 0);

        if (isSliding)
        {
            extraJumps = maxJumps;
            rb.gravityScale = gravityValue;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -slideSpeed, float.MaxValue));
            wallJumpingDir = (-transform.localScale.x);

        }

        if (Time.time - lastJumpInput < jumpGraceTime && extraJumps > 0 && isSliding)
        {
            isJumping = true;
            StartCoroutine(Jump());
        }
    }
    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (FacingRight && playerHorizontalInput < -movementBuffer || !FacingRight && playerHorizontalInput > movementBuffer)
        {
            FacingRight = !FacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        else if (isWallJumping)
        {
            FacingRight = !FacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    private void SetAnimation()
    {
        // Idle animation
        if (isGrounded && playerHorizontalInput == 0)
        {
            animator.Play("MC_Idle");
        }

        // Running animation
        else if (isGrounded && Mathf.Abs(playerHorizontalInput) > 0)
        {
            animator.Play("MC_Movement");
        }

        // Jumping animation
        else if (isJumping)
        {
            animator.Play("MC_Jump");
        }
    }

    public void SetGlobalLight()
    {
        globalLightSource.intensity += .03f;
    }
    
}
