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


public class Player : MonoBehaviour
{
    public float playerHealth;
    public float playerSpeed;
    public float playerTimeStopped;

    public float playerHorizontalAcceleration;
    public float playerVerticalAcceleration;

    public float playerMaxSpeed;
    public float playerMaxHealth;
    public float playerMaxJumpHeight;
    public float playerMaxTimeToStop;

    public void updateHealth()
    {
        //we can make this more complex after for now
        if (playerSpeed == 0)
        {
            playerTimeStopped++;
            if (playerTimeStopped > playerMaxTimeToStop)
            {
                playerHealth--;
            }
        }
        else if (Math.Abs(playerSpeed) >= (playerMaxSpeed / 5))
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
        if ((Input.GetKey(KeyCode.A)) && (-playerSpeed <= playerMaxSpeed))
        {
            playerSpeed -= playerHorizontalAcceleration;
        }
        if ((Input.GetKey(KeyCode.D)) && (playerSpeed <= playerMaxSpeed))
        {
            playerSpeed += playerHorizontalAcceleration;
        }
        if (!(Input.GetKey(KeyCode.A)) && !(Input.GetKey(KeyCode.D)))
        {
            if (playerSpeed != 0)
            {
                playerSpeed -= 10 * playerSpeed * playerHorizontalAcceleration;
            }
        }
    }

    bool isPositionInBounds()
    {
        //hard-coding here to change after
        return (transform.position.x > -7.5 && transform.position.x < 7.5);
    }

    void updateModel()
    {
        if (isPositionInBounds())
        {
            transform.position = new Vector2(transform.position.x + playerSpeed, transform.position.y);
        }
        

        if (playerSpeed >= 0)
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
        playerSpeed = 0;
        playerTimeStopped = 0;

        playerHorizontalAcceleration = 0.02f;
        playerVerticalAcceleration = -0.05f;

        playerMaxSpeed = 0.05f;
        playerMaxHealth = 10 * playerMaxSpeed;
        playerMaxJumpHeight = 5;
        playerMaxTimeToStop = 200;
    }

    // Update is called once per frame
    void Update()
    {
        updateSpeed();
        updateModel();
        updateHealth();

    }
}
