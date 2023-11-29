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
    [SerializeField] int numLives;

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
            numLives -= 1;
            if (numLives > 0)
            {
                Destroy(livesArray[numLives]);
            }
            else
            {
                Debug.Log("No more lives - Death");
            }
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
