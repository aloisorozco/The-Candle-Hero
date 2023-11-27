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
            collision.gameObject.GetComponent<PlayerMovement>().RespawnPlayer();
        }
    }

    public void setRespawn(Transform newRespawn)
    {
        respawnPoint = newRespawn;
    }

    public Transform getRespawn()
    {
        return respawnPoint;
    }
}
