using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderTrigger : MonoBehaviour
{

    [SerializeField] private bool hasEnteredBefore = false;
    [SerializeField] private GameObject boulder;

    [SerializeField] private Vector2 force = new Vector2(-2000, 0);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasEnteredBefore)
        {
            hasEnteredBefore = true;
            GameObject boulderObject = Instantiate(boulder, boulder.transform);
            boulderObject.GetComponent<Boulder>().force = force;
        }
    }
}
