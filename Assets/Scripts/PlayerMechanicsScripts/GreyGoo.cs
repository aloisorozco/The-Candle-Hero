using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //TODO: test values for range & speed

    void Start()
    {

    }

    void Update()
    {
        if (playerTransform != null)
        {
            if (Vector2.Distance(playerTransform.position, gameObject.transform.position) < mFollowRange)
            {
                gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, playerTransform.position, mFollowSpeed);
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
}
