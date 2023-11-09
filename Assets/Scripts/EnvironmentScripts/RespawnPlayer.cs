using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform respawnPoint;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.gameObject.transform.position = respawnPoint.position;
        }
    }

    public void setRespawn(Transform newRespawn)
    {
        respawnPoint = newRespawn;
    }

    public Transform getRespawnPoint()
    {
        return respawnPoint;
    }
}
