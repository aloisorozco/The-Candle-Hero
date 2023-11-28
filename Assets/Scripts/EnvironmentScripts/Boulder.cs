using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    [SerializeField] public Vector2 force = new Vector2(0f, 0f);
    private bool hasCollidedBefore = false;

    public void Update()
    {
        this.GetComponent<Rigidbody2D>().AddForce(force);
    }

    private IEnumerator FreezePlayer(GameObject player)
    {
        PlayerMovement playerScript = player.GetComponent<PlayerMovement>();

        playerScript.SetSpeedMultiplier(0.001f);
        playerScript.SetFrozen(true);

        yield return new WaitForSeconds(1f);

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

            col.gameObject.GetComponent<Rigidbody2D>().AddForce(playerForce * force.magnitude);
            StartCoroutine(FreezePlayer(col.gameObject));
        }
    }
}
