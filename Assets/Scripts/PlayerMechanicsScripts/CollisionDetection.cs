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
    public DataManager dataManager;
    public bool canTalk;
    private string currentSceneName;
    private bool onDoor;
    [SerializeField] private ImpactFlash impactFlash;
    [SerializeField] private Canvas onScreenText;
    [SerializeField] private Canvas UI_particles;

    private void Start()
    {
        player = GetComponent<PlayerMovement>();

        if (FindAnyObjectByType<DataManager>())
        {
            dataManager = FindAnyObjectByType<DataManager>();
        }
    }

    private void Update()
    {
        if (onDoor && Input.GetKeyDown(KeyCode.E))
        {
            dataManager.data.currentScene = currentSceneName;
            dataManager.data.respawnPoint = "InitialRespawnPoint";
            FindAnyObjectByType<MusicPlayer>().levelMusic.Stop();
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
                resourceManager.AddEmber();
                dataManager.SetDashUpgrade();
                UI_particles.GetComponentInChildren<ParticleSystem>().Play();
            }
            player.SetGlobalLight(collision.GetComponent<CandleInformation>().lightValue);
            collision.GetComponent<CircleCollider2D>().enabled = false;
            collision.GetComponentInChildren<ParticleSystem>().Play();
            respawn.setRespawn(collision.gameObject.name);
            StartCoroutine(impactFlash.FlashRoutine());

            dataManager.SavePlayer();
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

        else if (collision.CompareTag("NPC"))
        {
            canTalk = true;
            onScreenText.enabled = true;
            onScreenText.transform.position = collision.transform.position + Vector3.up * 1.6f;
            onScreenText.GetComponentInChildren<TMP_Text>().text = "Talk";
        }
        else if (collision.CompareTag("Altar"))
        {
            onScreenText.enabled = true;
            onScreenText.transform.position = collision.transform.position + Vector3.up * 2.0f;
            onScreenText.GetComponentInChildren<TMP_Text>().text = "Buy Upgrades";
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
        else if (collision.CompareTag("NPC"))
        {
            canTalk = false;
            onScreenText.enabled = false;
        }
        else if (collision.CompareTag("Altar"))
        {
            onScreenText.enabled = false;
        }
    }

}
