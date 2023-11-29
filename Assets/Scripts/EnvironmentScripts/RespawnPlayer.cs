using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class RespawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] public GameObject playerUI;
    [SerializeField] public GameObject heartsPrefab;
    [SerializeField] public int numLives;

    private DataManager dataManager;
    private GameObject[] livesArray;
    private void Start()
    {
        if (FindAnyObjectByType<DataManager>())
        {
            dataManager = FindAnyObjectByType<DataManager>();
            GameObject respawnObject = GameObject.Find(dataManager.data.respawnPoint);
            if (dataManager.data.respawnPoint == "InitialRespawnPoint")
            {
                respawnPoint = respawnObject.transform;
            }
            else
            {
                respawnPoint = respawnObject.transform.Find("SpawnPoint").transform;
            }
            livesArray = new GameObject[numLives];
            for (int i = 0; i < numLives; i++)
            {
                if (livesArray[i] == null)
                    livesArray[i] = Instantiate(heartsPrefab, new Vector3(44 + (88 * i), 44, 0), Quaternion.identity, playerUI.transform);
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
        if (newRespawn != "InitialRespawnPoint")
        {
            respawnPoint = respawnObject.transform.Find("SpawnPoint").transform;
        }
        else
        {
            respawnPoint = respawnObject.transform;
        }

        if (dataManager)
        {
            dataManager.SetRespawnPoint(newRespawn);
        }
    }

    public Transform getRespawn()
    {
        return respawnPoint;
    }

    public void removeHeart()
    {
        numLives--;
        if (numLives > 0)
        {
            Destroy(livesArray[numLives]);
        }
        else
        {
            Destroy(livesArray[0]);
            Debug.Log("No more lives - Death");
        }
    }
}
