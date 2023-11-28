using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//source: https://www.youtube.com/watch?v=RuvfOl8HhhM

public class EnemyAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform currentPoint;
    [SerializeField] private float speed;

    [Header("Knockback and Freeze Settings")]
    [SerializeField] private float knockbackForce;
    [SerializeField] private float timeFrozen;

    private bool hasCollidedBefore = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
    }

    void Update()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == pointB.transform)
        {
            transform.localScale = new Vector3(-4, 4, 1);
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            transform.localScale = new Vector3(4, 4, 1);
            rb.velocity = new Vector2(-speed, 9);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            hasCollidedBefore = false;
            currentPoint = pointA.transform;
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            hasCollidedBefore = false;
            currentPoint = pointB.transform;
        }
    }

    private void OnDrawGizmos() { 
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);

        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }

    private IEnumerator FreezePlayer(GameObject player)
    {
        PlayerMovement playerScript = player.GetComponent<PlayerMovement>();

        playerScript.SetSpeedMultiplier(0.001f);
        playerScript.SetFrozen(true);

        yield return new WaitForSeconds(timeFrozen);

        playerScript.SetSpeedMultiplier((1f / 0.001f));
        playerScript.SetFrozen(false);
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" && !hasCollidedBefore)
        {
            hasCollidedBefore = true;
            Vector2 playerForce = (col.gameObject.transform.position - transform.position);
            playerForce.Normalize();

            col.gameObject.GetComponent<Rigidbody2D>().AddForce(playerForce * knockbackForce);
            StartCoroutine(FreezePlayer(col.gameObject));
        }
    }

}
