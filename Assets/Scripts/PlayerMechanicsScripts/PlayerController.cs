using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    public float playerHorizontalAcceleration;
    public float playerVerticalAcceleration;

    public float playerMaxSpeed;
    public float playerMaxHealth;
    public float playerMaxJumpHeight;
    public float playerMaxTimeToStop;

    public bool isJumping;

    public void updateHealth()
    {
        //we can make this more complex after for now
        if (playerSpeed.x == 0)
        {
            playerTimeStopped++;
            if (playerTimeStopped > playerMaxTimeToStop)
            {
                playerHealth--;
            }
        }
        else if (Math.Abs(playerSpeed.x) >= (playerMaxSpeed / 5))
        {
            playerTimeStopped = 0;
            if (playerHealth < playerMaxHealth)
            {
                playerHealth++;
            }
        }
    }

    void updateSpeed()
    {
        //we can configure the movement keys in project settings later
        if ((Input.GetKey(KeyCode.A)) && (-playerSpeed.x <= playerMaxSpeed))
        {
            playerSpeed.x -= playerHorizontalAcceleration;
        }
        if ((Input.GetKey(KeyCode.D)) && (playerSpeed.x <= playerMaxSpeed))
        {
            playerSpeed.x += playerHorizontalAcceleration;
        }
        if (!(Input.GetKey(KeyCode.A)) && !(Input.GetKey(KeyCode.D)))
        {
            if (playerSpeed.x != 0)
            {
                playerSpeed.x -= 10 * playerSpeed.x * playerHorizontalAcceleration;
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

    //TODO: set up rigid bodies of level walls & floors
    private void OnCollisionEnter(Collision other)
    {
        playerVerticalAcceleration = 0f;
        isJumping = false;
    }

    private void OnCollisionExit(Collision other)
    {
        playerVerticalAcceleration = -0.05f;
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
        

        if (playerSpeed.x >= 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        
        
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
        playerMaxHealth = 10 * playerMaxSpeed;
        playerMaxJumpHeight = 5;
        playerMaxTimeToStop = 200;

        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        updateSpeed();
        updateModel();
        updateHealth();

    }
}
