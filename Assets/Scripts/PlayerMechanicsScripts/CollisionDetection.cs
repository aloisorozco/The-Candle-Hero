using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionDetection : MonoBehaviour
{

    public PlayerMovement player;
    public RespawnPlayer respawn;
    public ResourceManager resourceManager;
    private string currentSceneName;
    private bool onDoor;
    [SerializeField] private ImpactFlash impactFlash;
    [SerializeField] private Canvas onScreenText;
    [SerializeField] private Canvas UI_particles;

    private void Start()
    {
        player = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (onDoor && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(currentSceneName);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            if (!collision.GetComponent<CandleInformation>().hasVisitedBefore)
            {
                collision.GetComponent<CandleInformation>().hasVisitedBefore = true;
                resourceManager.AddCountCandle(collision.GetComponent<CandleInformation>().value);
                UI_particles.GetComponentInChildren<ParticleSystem>().Play();
            }
            player.SetGlobalLight(collision.GetComponent<CandleInformation>().lightValue);
            collision.GetComponent<CircleCollider2D>().enabled = false;
            collision.GetComponentInChildren<ParticleSystem>().Play();
            respawn.setRespawn(collision.transform);
            StartCoroutine(impactFlash.FlashRoutine());
        }
        else if (collision.CompareTag("Door"))
        {
            onDoor = true;
            onScreenText.enabled = true;
            onScreenText.transform.position = collision.transform.position;
            onScreenText.GetComponentInChildren<TMP_Text>().text = "Enter " + collision.GetComponent<DoorInformation>().doorName;
            currentSceneName = collision.GetComponent<DoorInformation>().sceneName;
        }
        else if (collision.CompareTag("Cobweb"))
        {
            player.SetSpeedMultiplier(0.55f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            onDoor = false;
            onScreenText.enabled = false;
        }
        else if (collision.CompareTag("Cobweb"))
        {
            player.SetSpeedMultiplier((1.0f / 0.55f));
        }
    }

}
