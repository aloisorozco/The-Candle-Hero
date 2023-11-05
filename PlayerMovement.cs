using System.Collections;
using System.Collections.Generic;
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
    public bool onWall;
    private bool lastOnWall;
    public bool isSliding;
    private bool isWallJumping;
    private float wallJumpingDir;
    [Header("Wall Settings")]
    [SerializeField] private float slideSpeed;
    [SerializeField] private float wallJumpingDuration = 0.15f;
    [SerializeField] private float wallJumpForce = 10f;
    [SerializeField] private float wallJumpGraceTime = .05f;

    [Header("Attack Settings")]
    [SerializeField] private bool isAttacking;
    [SerializeField] private float attackDuration = 0.5f;

    [Header("Dash Settings")]
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashTime;
    [SerializeField] private float downDashForce;
    [SerializeField] private float dashJumpGraceTime = 0.5f;
    public bool canDash = true;
    private bool isDashing;


    private float CheckRadius = .2f;

    //Animation States
    private string currentAnimation;
    const string PLAYER_IDLE = "Player_Idle";
    const string PLAYER_RUN = "Player_Run";
    const string PLAYER_JUMP = "Player_Jump";
    const string PLAYER_IDLE_JUMP = "Player_Idle_Jump";
    const string PLAYER_FALL = "Player_Fall";
    const string PLAYER_IDLE_FALL = "Player_Idle_Fall";
    const string PLAYER_WALL_PUSH = "Player_Wall_Push";
    const string PLAYER_WALL_SLIDE = "Player_Wall_Slide";
    const string PLAYER_ATTACK = "Player_Attack";

    //Components
    private Rigidbody2D rb;
    public AnimationScript ani;
    private PlayerAttack pa;
    private TrailRenderer tr;

    //Time Variables
    private float lastGroundedTime =0f;
    private float lastWalledTime = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pa = GetComponent<PlayerAttack>();
        tr = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // checking for player state
        isPlayerGrounded();
        isOnWall();
        isWallSliding();

        // Checking for user input
        if (!isWallJumping && !isDashing)
        {
            InputManager();
        }
        DefaultInputManager();
        // Setting up animation variables
        setStateAnimation();
        
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


    }

    private void InputManager()
    {
        playerHorizontalInput = Input.GetAxisRaw("Horizontal") * runSpeed;

        
        if ( Input.GetButtonDown("Jump"))
        {
            StartCoroutine(Jump());
        }
        if (Input.GetButtonDown("Fire1") && !isSliding)
        {
            pa.Attack(attackDuration);
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

        if (Input.GetButtonDown("Fire3") && canDash)
        {
            StartCoroutine(Dash());
        }
        if (Input.GetButtonDown("Crouch") && !isGrounded)
        {
            StartCoroutine(Dash());
        }

    }

    private void Run(float move)
    {
        Debug.Log(move);
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

        if(lastGroundedTime>0 && Mathf.Abs(move) < movementBuffer)
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
        else if(isGrounded || Time.time - lastGroundedTime < coyoteTime)
        {
            rb.gravityScale = gravityValue;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        else if(!isGrounded && extraJumps > 0)
        {
            extraJumps--;
            rb.gravityScale = gravityValue;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        lastGroundedTime = 0f;
        lastJumpTime = Time.time;
        isJumping = true;
        tr.emitting = true;
        //canJump = false;
        yield return new WaitForSeconds(jumpTime);
        tr.emitting = false;
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
        if(Input.GetAxisRaw("Vertical") < -0.5f && !isGrounded)
        {
            rb.velocity = new Vector2(0.0f, - downDashForce);
        }
        else
        {
            rb.velocity = new Vector2(transform.localScale.x * dashForce, 0f);
        }
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        if (Time.time - lastJumpInput < dashJumpGraceTime && extraJumps > 0 && isGrounded)// && canJump)
        {
            StartCoroutine(Jump());
        }
        tr.emitting = false;
        rb.gravityScale= gravityValue;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        

    }

    
    private void stopJump()
    {
        if(rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x , rb.velocity.y * stopJumpForce);
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
            || Physics2D.OverlapCircle(GroundCheckLeft.position, CheckRadius, WhatIsGround);

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
        
        if(onWall)
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
        if(FacingRight && playerHorizontalInput < -movementBuffer || !FacingRight && playerHorizontalInput > movementBuffer)
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

    private void setStateAnimation()
    {
        bool isTryingToMove = Mathf.Abs(playerHorizontalInput) > 0;
        bool isMoving = rb.velocity.x < -0.1f || rb.velocity.x > 0.1f;
        bool isAttacking = pa.isAttacking;
        bool isFalling = rb.velocity.y < 0f;

        if (isAttacking)// When player attacks, all animations stop
        {
            ani.SetAnimationState(PLAYER_ATTACK);
        }
        else if (isGrounded)// if player is grounded
        {
            if (onWall)
            {
                if (isTryingToMove)
                {
                    ani.SetAnimationState(PLAYER_WALL_PUSH);
                }
                else
                {
                    ani.SetAnimationState(PLAYER_IDLE);
                }
            }
            else
            {
                if (isMoving)
                {
                    ani.SetAnimationState(PLAYER_RUN);
                }
                else
                {
                    ani.SetAnimationState(PLAYER_IDLE);
                }
            }
        }
        else if(!isGrounded)
        {
            if (isSliding)
            {
                ani.SetAnimationState(PLAYER_WALL_SLIDE);
            }
            else if (isFalling)
            {
                if (isTryingToMove)
                {
                    ani.SetAnimationState(PLAYER_FALL);
                }
                else
                {
                    ani.SetAnimationState(PLAYER_IDLE_FALL);
                } 
            }
            else if (onWall)
            {
                if (isTryingToMove)
                {
                    ani.SetAnimationState(PLAYER_JUMP);
                }
                else
                {
                    ani.SetAnimationState(PLAYER_IDLE_JUMP);
                }
            }
            else
            {
                if (isMoving)
                {
                    ani.SetAnimationState(PLAYER_JUMP);
                }
                else
                {
                    ani.SetAnimationState(PLAYER_IDLE_JUMP);
                }
            }
        }
        else
        {
            ani.SetAnimationState(PLAYER_IDLE);
        }

        
    }
}
