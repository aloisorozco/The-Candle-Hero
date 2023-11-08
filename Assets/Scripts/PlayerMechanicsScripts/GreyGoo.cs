using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/*
TODO:
-add sprite to gameobject
-implement ais for enemies:
    -make it go to player if within a range like in lab
    -make it bounce back and forth between an area
-theres def more i just cant think of it rn
*/

public class GreyGoo : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;
    [SerializeField]
    float mFollowSpeed;
    [SerializeField]
    float mFollowRange;

    Vector2[] pointsToCover = new Vector2[] {
        new Vector2(0, 1),
        new Vector2(3, 1)
    };
    int pointToCoverIndex = 0;
    int delayCount = 0;

    public float enemyHealth = 50f;

    // Animation
    public Animator anim;
    public SpriteRenderer spriteRenderer;
    //TODO: test values for range & speed

    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (playerTransform != null)
        {
            if (Vector2.Distance(playerTransform.position, gameObject.transform.position) < mFollowRange)
            {
                if (this.transform.position.x < playerTransform.position.x)
                    FaceDirection(Vector2.right);
                else
                    FaceDirection(Vector2.left);
                transform.position = Vector2.MoveTowards(this.transform.position, playerTransform.position, mFollowSpeed * Time.deltaTime);
            }
        }

        if ((delayCount % 30) == 0)
        {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, pointsToCover[(pointToCoverIndex % 2)], mFollowSpeed);
            pointToCoverIndex++;
        }
        delayCount++;
    }

    public void SetTarget(Transform target)
    {
        playerTransform = target;
    }

    private void FaceDirection(Vector2 direction)
    {
        // Flip the sprite
        if (direction == Vector2.right)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }
}
