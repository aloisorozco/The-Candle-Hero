using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
    [SerializeField] private float wallStopTime = 0.5f;
    [SerializeField] private bool isWallStop = false;
    [SerializeField] private bool wasOnWall = false;


    [Header("Dash Settings")]
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashTime;
    [SerializeField] private float downDashForce;
    [SerializeField] private float dashJumpGraceTime = 0.5f;
    [SerializeField] private float dashStopForce;
    [SerializeField] private int nbDashInAir = 1;
    [SerializeField] private int maxDashInAir = 1;
    private bool canDash = true;
    private bool isDashing;


    [Header("Light Settings")]
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D lightSource;
    [SerializeField] public UnityEngine.Rendering.Universal.Light2D globalLightSource;
    [SerializeField] private float lightMin = 2f;
    [SerializeField] private float lightMax = 5f;
    [SerializeField] private float globalLightMin = 0.05f;
    [SerializeField] private float globalLightMax = 0.25f;
    [SerializeField] private float lightRate = .2f;
    [SerializeField] private float globalLightRate = .01f;

    [Header("Health Settings")]
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth = 100;
    [SerializeField] private int increaseHealthRate = 5;
    [SerializeField] private int decreaseHealthRate = 5;
    [SerializeField] private int currentLives = 3;
    [SerializeField] private int maxLives = 3;
    [SerializeField] private GameObject respawn;
    [SerializeField] private GameObject candles;
    [SerializeField] private Transform initialRespawnPoint;

    [Header("Death Settings")]
    [SerializeField] private Canvas deathScreen;
    [SerializeField] public bool isDead;
    [SerializeField] private AudioSource deathSound;
    private bool deathSoundPlaying = false;


    [Header("Safe Area Settings")]
    [SerializeField] private GameObject safeArea;
    [SerializeField] private bool inSafeArea;

    [Header("Animator and Sound")]
    [SerializeField] public Animator animator;
    [SerializeField] private AudioSource runningAudio;

    [Header("Idle Settings")]
    [SerializeField] private int maxTimeIdleBeforeLosingHealth = 50;
    [SerializeField] private float idleEpsilon = 3f;
    private int timeIdleCount = 0;

    private float CheckRadius = .2f;

    [Header("Upgrade Settings")]
    public bool dashUpgrade = false;
    public bool doubleJumpUpgrade = false;
    public bool wallJumpUpgrade = false;

    [Header("Data Settings")]
    public DataManager dataManager;

    //Components
    private Rigidbody2D rb;

    //Time Variables
    private float lastGroundedTime = 0f;
    private float lastWalledTime = 0f;

    private bool isFrozen = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBar.SetHealth(currentHealth, maxHealth);
        lightSource.pointLightOuterRadius = lightMin;
        

    }

    private void Start()
    {
        transform.position = respawn.GetComponent<RespawnPlayer>().getRespawn().position;
    }

    // Update is called once per frame
    void Update()
    {
        // checking for player state
        isPlayerGrounded();
        isOnWall();
        if (wallJumpUpgrade)
        {
            isWallSliding();
        }
        SetAnimation();
       
        if (!isDead)
        {
            // Checking for user input
            if (!isWallJumping && !isDashing)
            {
                InputManager();
            }
            DefaultInputManager();
        }
        
        // Setting up animation variables

        if (dataManager)
        {
            dashUpgrade = dataManager.data.dashUpgrade;
            doubleJumpUpgrade = dataManager.data.doubleJumpUpgrade;
            wallJumpUpgrade = dataManager.data.wallJumpUpgrade;
        }


        SetData();
    }

    private IEnumerator WallStop(float wallStopDuration)
    {
        isDashing = false;
        StopCoroutine(Dash());
        isWallStop = true;
        rb.velocity = new Vector2(0f, 0f);
        rb.gravityScale = 0f;
        yield return new WaitForSeconds(wallStopDuration);
        rb.gravityScale = gravityValue;
        isWallStop = false;
    }

    private void FixedUpdate()
    {
        if (isDashing) { return; }

        if (!isWallJumping && !isWallStop)
        {
            Run(playerHorizontalInput * Time.fixedDeltaTime);
        }
        if (!isWallJumping && !isDashing)
        {
            Flip();
        }

        inSafeArea = safeArea.GetComponent<SafeArea>().inSafeArea;
        if (inSafeArea)
        {
            currentHealth = maxHealth;
            healthBar.SetHealth(currentHealth, maxHealth);
            globalLightSource.intensity = Mathf.Clamp(globalLightSource.intensity + globalLightRate, globalLightMin, globalLightMax);
            return;
        }

        if(SceneManager.GetActiveScene().name != "Tutorial" && !isDead)
        {
            SetHealth();
        }
        if (!isDead)
        {
            SetLight();
            SetTimeIdle();
            SetLives();
        }

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
        globalLightSource.intensity = Mathf.Clamp(globalLightSource.intensity - globalLightRate, globalLightMin, globalLightMax);
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
                currentHealth += increaseHealthRate;
            }
        }
        else
        {
            if((currentHealth > 0) && (timeIdleCount >= maxTimeIdleBeforeLosingHealth))
            {
                currentHealth -= decreaseHealthRate;
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

    public void RespawnPlayer()
    {
        currentLives--;
        dataManager.data.lives--;
        respawn.GetComponent<RespawnPlayer>().removeHeart();

        if (currentLives == 0) 
        {
            StartCoroutine(Death());
        }
        else
        {
            currentHealth = maxHealth;
            timeIdleCount = 0;

            rb.velocity = Vector2.zero;
            transform.position = respawn.GetComponent<RespawnPlayer>().getRespawn().position;
        }
    }

    

    private void SetLives()
    {
        MusicPlayer musicPlayer = FindAnyObjectByType<MusicPlayer>();
        if (currentHealth < maxHealth && !deathSoundPlaying)
        {
            deathSoundPlaying = true;
            musicPlayer.deathSound(true, deathSound);
        }
        else if (currentHealth == maxHealth && deathSoundPlaying)
        {
            deathSoundPlaying = false;
            musicPlayer.deathSound(false, deathSound);
        }

        if (currentHealth <= 0)
        {
            if (currentLives-1 > 0 && deathSoundPlaying)
            {
                deathSoundPlaying = false;
                musicPlayer.deathSound(false, deathSound);
            }
            RespawnPlayer();
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

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && dashUpgrade && isGrounded && !onWall)
        {
            StartCoroutine(Dash());
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isGrounded && canDash && dashUpgrade && nbDashInAir > 0 && !onWall)
        {
            nbDashInAir--;
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
            //Change acceleration depending on situation
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;

            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

            rb.AddForce(movement * Vector2.right);
        }

        if (isGrounded && Mathf.Abs(move) <= movementBuffer)
        {
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(rb.velocity.x);
            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
        if (!isGrounded && Mathf.Abs(move) <= movementBuffer)
        {
            float targetSpeed = move * runSpeed;
            //Find the difference between our current speed and the disired speed
            float speedDif = targetSpeed - rb.velocity.x;
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * decceleration, velPower) * -Mathf.Sign(rb.velocity.x);

            rb.AddForce(movement * Vector2.right);
        }
    }

    private IEnumerator Jump()
    {
        if (onWall && !isGrounded && wallJumpUpgrade)
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
        else if (!isGrounded && extraJumps > 0 && doubleJumpUpgrade)
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
        rb.velocity = new Vector2(0f, 0f);
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
        rb.velocity = new Vector2(transform.localScale.x * dashForce, 0f);
        float stopforce = Mathf.Pow(Mathf.Abs(rb.velocity.x) * dashStopForce, velPower) * -transform.localScale.x;
        yield return new WaitForSeconds(dashTime);
        if (!onWall)
        {
            rb.AddForce(stopforce * Vector2.right);
        }


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
            nbDashInAir = maxDashInAir;
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
            isDashing = false;
            StopCoroutine(Dash() );
            extraJumps = maxJumps;
            nbDashInAir = maxDashInAir;
            isJumping = false;
            rb.gravityScale = gravityValue;
            wallJumpingDir = (-transform.localScale.x);
            if (!isGrounded && wallJumpUpgrade) 
            {
                StartCoroutine(WallStop(wallStopTime));
            }

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

        if (isSliding && !isWallStop)
        {
            extraJumps = maxJumps;
            rb.gravityScale = gravityValue;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -slideSpeed, float.MaxValue));
            

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
        if ((FacingRight && playerHorizontalInput < -movementBuffer) || (!FacingRight && playerHorizontalInput > movementBuffer) )
        {
            if (!isWallStop)
            {
                FacingRight = !FacingRight;
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
            }
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
        //Idle Animation if MC Frozen
        if (isFrozen)
        {
            animator.Play("MC_Hurt");
            return;
        }

        if (isDead)
        {
            animator.Play("MC_Death");
            return;
        }

        // Idle animation
        if (isGrounded && playerHorizontalInput == 0)
        {
            animator.Play("MC_Idle");


            // Settings Sounds
            runningAudio.enabled = false;
        }

        // Running animation
        else if (isGrounded && Mathf.Abs(playerHorizontalInput) > 0)
        {
            animator.Play("MC_Movement");


            // Settings Sounds
            runningAudio.enabled = true;
        }

        // Jumping animation
        else if (isJumping && !isSliding)
        {
            animator.Play("MC_Jump");


            // Settings Sounds
            runningAudio.enabled = false;
        }
        else if (onWall && isSliding)
        {
            animator.Play("MC_WallGrip");
        }
        else
        {
            runningAudio.enabled = false;
        }
    }

    public void SetGlobalLight(float value)
    {
        globalLightSource.intensity += value;
    }


    public void SetData()
    {
        if (FindAnyObjectByType<DataManager>())
        {
            dataManager = FindAnyObjectByType<DataManager>();
            dashUpgrade = dataManager.data.dashUpgrade;
            doubleJumpUpgrade = dataManager.data.doubleJumpUpgrade;
            wallJumpUpgrade = dataManager.data.wallJumpUpgrade;
            currentLives = dataManager.data.lives;
            lightMax = dataManager.data.lightRadius;

        }
    }

    public void getNewAbility()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        UpgradeManager upgradeManager = FindAnyObjectByType<UpgradeManager>();
        string abilityName;
        string abilityDescription;

        if (currentScene.name == "Level_1" && !dashUpgrade)
        {
            abilityName = "Gust of Steam";
            abilityDescription = "Press SHIFT to dash forwards.";
            upgradeManager.openAbilityPopUp(abilityName, abilityDescription);
            dataManager.SetDashUpgrade();
            dataManager.SavePlayer();
        }
        if (currentScene.name == "Level_2" && !wallJumpUpgrade)
        {
            abilityName = "Ashen Gloves";
            abilityDescription = "Press SPACE on a wall to wall jump.";
            upgradeManager.openAbilityPopUp(abilityName, abilityDescription);
            dataManager.SetWallJumpUpgrade();
            dataManager.SavePlayer();
        }
        if (currentScene.name == "Level_3" && !doubleJumpUpgrade)
        {
            abilityName = "Fire Jump";
            abilityDescription = "Press SPACE in the air to perform a double jump.";
            upgradeManager.openAbilityPopUp(abilityName, abilityDescription);
            dataManager.SetDoubleJumpUpgrade();
            dataManager.SavePlayer();
        }
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        runSpeed *= multiplier;
    }

    public void SetJumpMultiplier(float multiplier)
    {
        jumpForce *= multiplier;
    }
    public void SetDashMultiplier(float multiplier)
    {
        dashForce *= multiplier;
    }

    public void SetGlobalLightIntensity(float intensity)
    {
        globalLightSource.intensity = intensity;
    }

    public void SetMaxLightRadius(float radius)
    {

        lightMax = radius;
    }
    public void SetLightSetGlobalLightIntensity(float intensity)
    {
        lightSource.intensity = intensity;
    }
    public void AddLife(int value)
    {
        maxLives += value;
        currentLives += value;
    }

    public void AddHealingEmber()
    {
        increaseHealthRate = 10;
        decreaseHealthRate = 2;
    }

    public void SetFrozen(bool frozen)
    {
        isFrozen = frozen;
    }

    private IEnumerator Death()
    {

        isDead = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(2);

        deathScreen.enabled = true;
        yield return new WaitForSeconds(5);

        respawn.GetComponent<RespawnPlayer>().setRespawn("InitialRespawnPoint");
        dataManager.data.lives = maxLives;
        currentLives = maxLives;
        candles.GetComponent<Candles>().ResetCandles();
        currentHealth = maxHealth;
        timeIdleCount = 0;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isDead = false;
    }
}
