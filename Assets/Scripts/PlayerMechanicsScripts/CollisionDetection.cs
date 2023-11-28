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
    private bool onShop;
    [SerializeField] private ImpactFlash impactFlash;
    [SerializeField] private Canvas onScreenText;
    [SerializeField] private Canvas UI_particles;
    [SerializeField] private Canvas ShopUI;

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
        if(onShop && Input.GetKeyDown(KeyCode.E))
        {
            EnableShop();
        }
    }

    private void EnableShop()
    {
        Time.timeScale = 0f;
        ShopUI.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            player.SetGlobalLight(collision.GetComponent<CandleInformation>().lightValue);
            collision.GetComponent<CircleCollider2D>().enabled = false;
            collision.GetComponentInChildren<ParticleSystem>().Play();
            UI_particles.GetComponentInChildren<ParticleSystem>().Play();
            respawn.setRespawn(collision.transform);
            resourceManager.AddCountCandle(collision.GetComponent<CandleInformation>().value);
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
        else if (collision.CompareTag("UpgradeShop"))
        {
            onShop = true;
            onScreenText.enabled = true;
            onScreenText.transform.position = collision.transform.position;
            onScreenText.GetComponentInChildren<TMP_Text>().text = "Enter Upgrade Shop";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            onDoor = false;
            onScreenText.enabled = false;
        }
        if (collision.CompareTag("UpgradeShop"))
        {
            onShop = false;
            onScreenText.enabled = false;
        }
    }

}
