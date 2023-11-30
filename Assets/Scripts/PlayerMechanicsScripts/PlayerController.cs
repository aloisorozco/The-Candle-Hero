using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using UnityEngine.UI;
/*
TODO:
-for vertical movement, we need to implement rigidbody in the scene
    (i will write the code but comment it out until that is done)
-test with best values for speed, health, time allowed to stop, etc.
-integrate health variable with health bar
-configure keys in project settings
-update updateHealth function to make it more interesting/complex
-much more but for now all i can think of
*/


public class PlayerController : MonoBehaviour
{
    public float playerHealth;
    public Vector2 playerSpeed;
    public float playerTimeStopped;

    public float playerHorizontalAcceleration = 0.2f;
    public float playerVerticalAcceleration;

    public float playerMaxSpeed;
    public float playerMaxHealth;
    public float playerMaxJumpHeight;
    public float playerMaxTimeToStop;


    public bool isJumping;

    // Animation
    public bool isMoving;
    public SpriteRenderer spriteRenderer;
    public Animator anim;
    Vector2 mFacingDirection;

    // Slider for health
    public Slider slider;

    public void updateHealth()
    {
        //we can make this more complex after for now
        if (Math.Abs(playerSpeed.x) <= (playerMaxSpeed / 1000))
        {
            playerTimeStopped++;
            if (playerTimeStopped > playerMaxTimeToStop)
            {
                playerHealth--;
            }
        }
        else if (Math.Abs(playerSpeed.x) >= (playerMaxSpeed / 1000))
        {
            playerTimeStopped = 0;
            if (playerHealth < playerMaxHealth)
            {
                playerHealth++;
            }
        }

        if (playerHealth <= 0)
        {
            //TODO: remove player
            // Debug.Log("player dead");
            playerHealth = 0;
        }
        slider.value = playerHealth;
    }

    public Vector2 GetFacingDirection()
    {
        return mFacingDirection;
    }

    void UpdateSpeed()
    {
        //we can configure the movement keys in project settings later
        if ((Input.GetKey(KeyCode.A)) && (-playerSpeed.x <= playerMaxSpeed))
        {
            FaceDirection(Vector2.left);
            playerSpeed.x -= playerHorizontalAcceleration;
            
            isMoving = true;
        }
        if ((Input.GetKey(KeyCode.D)) && (playerSpeed.x <= playerMaxSpeed))
        {
            FaceDirection(Vector2.right);
            playerSpeed.x += playerHorizontalAcceleration;
            
            isMoving = true;
        }
        if (!(Input.GetKey(KeyCode.A)) && !(Input.GetKey(KeyCode.D)))
        {
            if (playerSpeed.x != 0)
            {
                playerSpeed.x -= 10 * playerSpeed.x * playerHorizontalAcceleration;
                isMoving = false;
            }
        }

        //gravity
        playerSpeed.y += playerVerticalAcceleration;

        //jumping
        //TODO: get how long space is held for to adjust how high or low jump is
        if ((Input.GetKeyDown(KeyCode.Space)) && (!isJumping))
        {
            playerSpeed.y = 0.5f;
            isJumping = true;
        }
        //TODO: comment out if statement below once on collision with floor is handled
        else if (transform.position.y <= -2)
        {
            playerSpeed.y = 0;
            isJumping = false;
        }
    }

    //TODO: fix later right we can go past bounds and then get stuck there
    bool isPositionInBounds()
    {
        //hard-coding here to change after
        return (transform.position.x > -7.5 && transform.position.x < 7.5);
    }

    void updateModel()
    {
        if (isPositionInBounds())
        {
            transform.position = new Vector2(transform.position.x + playerSpeed.x, transform.position.y + playerSpeed.y);
            
        }
        //TODO: remove this else code when rigid bodies are implemented
        else
        {
            playerSpeed.x = 0;
            if (transform.position.x < -7.5)
            {
                transform.position = new Vector2(-7.49f, transform.position.y + playerSpeed.y);
            }
            else if (transform.position.x > 7.5)
            {
                transform.position = new Vector2(7.49f, transform.position.y + playerSpeed.y);
            }
        }
        

        if (playerSpeed.x >= 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        
        
    }

    private void FaceDirection(Vector2 direction)
    {
        Debug.Log("Direction: " + direction);
        if (direction == Vector2.right)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;
    }

    private void updateAnimator()
    {
        //anim.SetBool("isMoving", isMoving);
        //anim.SetBool("isJumping", isJumping);
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(-3, -2);

        playerHealth = 100;
        playerSpeed = new Vector2(0,0);
        playerTimeStopped = 0;

        playerHorizontalAcceleration = 0.02f;
        playerVerticalAcceleration = -0.05f;

        playerMaxSpeed = 3;
        playerMaxHealth = playerHealth;
        playerMaxJumpHeight = 5;
        playerMaxTimeToStop = 200;

        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isJumping = false;
        isMoving = false;
        
        // Health slider
        slider.maxValue = playerMaxHealth;
        slider.minValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpeed();
        updateAnimator();
        updateModel();
        updateHealth();
        
    }
}
