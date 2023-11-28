using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform respawnPoint;

    [Header("Player Life")]
    [SerializeField] GameObject heartsPrefab;
    [SerializeField] Transform panel;
    [SerializeField] int numLives;

    [Header("Death UI")]
    [SerializeField] GameObject deathScreen;
    [SerializeField] GameObject playerUI;

    GameObject[] livesArray;

    private void Start()
    {
        livesArray = new GameObject[numLives];
        for (int i = 0; i < numLives; i++)
        {
            if (livesArray[i] == null)
                livesArray[i] = Instantiate(heartsPrefab, new Vector3(44 + (88*i), 44, 0), Quaternion.identity, panel);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.gameObject.transform.position = respawnPoint.position;
            numLives -= 1;
            if (numLives > 0)
            {
                Destroy(livesArray[numLives]);
            }
            else
            {
                Time.timeScale = 0f;
                playerUI.SetActive(false);
                deathScreen.SetActive(true);
            }   
        }
    }

    public void setRespawn(Transform newRespawn)
    {
        respawnPoint = newRespawn;
    }
}
