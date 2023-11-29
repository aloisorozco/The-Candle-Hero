using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform respawnPoint;

    private DataManager dataManager;

    private void Start()
    {
        if (FindAnyObjectByType<DataManager>())
        {
            dataManager = FindAnyObjectByType<DataManager>();
            GameObject respawnObject = GameObject.Find(dataManager.data.respawnPoint);
            if(dataManager.data.respawnPoint == "InitialRespawnPoint")
            {
                respawnPoint = respawnObject.transform;
            }
            else
            {
                respawnPoint = respawnObject.transform.Find("SpawnPoint").transform;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().RespawnPlayer();
        }
    }

    public void setRespawn(string newRespawn)
    {
        GameObject respawnObject = GameObject.Find(newRespawn);
        respawnPoint = respawnObject.transform.Find("SpawnPoint").transform;

        if (dataManager)
        {
            dataManager.SetRespawnPoint(newRespawn);
        }
    }

    public Transform getRespawn()
    {
        return respawnPoint;
    }
}
