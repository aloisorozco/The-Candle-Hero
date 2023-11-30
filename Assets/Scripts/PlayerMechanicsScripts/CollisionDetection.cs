using System;
using TMPro;
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
    private bool onAltar;
    [SerializeField] private ImpactFlash impactFlash;
    [SerializeField] public Canvas onScreenText;
    [SerializeField] private Canvas UI_particles;
    [SerializeField] private Canvas altarUI;

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
            SaveSystem.SavePlayer(dataManager.data, dataManager.data.currentScene);
            SceneManager.LoadScene(currentSceneName);
        }
        if (onAltar && Input.GetKeyDown(KeyCode.E))
        {
            onScreenText.enabled = false;
            if (altarUI)
            {
                altarUI.enabled = true;
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Checkpoint"))
        {
            if (!collision.GetComponent<CandleInformation>().hasVisitedBefore)
            {
                collision.GetComponent<CandleInformation>().hasVisitedBefore = true;
            }
            MusicPlayer musicPlayer = FindAnyObjectByType<MusicPlayer>();
            musicPlayer.deathSound(false, player.deathSound);
            player.SetGlobalLight(collision.GetComponent<CandleInformation>().lightValue);
            collision.GetComponent<CircleCollider2D>().enabled = false;
            collision.GetComponentInChildren<ParticleSystem>().Play();
            collision.transform.Find("Flame")?.gameObject.SetActive(true);
            collision.transform.Find("Small Flame")?.gameObject.SetActive(false);
            respawn.setRespawn(collision.gameObject.name);
            StartCoroutine(impactFlash.FlashRoutine());

            Debug.Log("Before");
            dataManager.ResetLives();
            if (SceneManager.GetActiveScene().name != "Tutorial")
            {
                respawn.ResetHearts();
            }
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
            player.SetSpeedMultiplier(0.3f);
            if (!dataManager.data.jumpPlus)
            {
                player.SetJumpMultiplier(0.6f);
            }
            if (!dataManager.data.dashPlus)
            {
                player.SetDashMultiplier(0.6f);
            }
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
            onAltar = true;
            onScreenText.enabled = true;
            onScreenText.transform.position = collision.transform.position + Vector3.up * 2f;
            onScreenText.GetComponentInChildren<TMP_Text>().text = "Buy Upgrades";
        }
        if (collision.CompareTag("Ember"))
        {
            string number = collision.gameObject.name.Split(' ')[1];
            resourceManager.AddEmber(Int32.Parse(number));
            UI_particles.GetComponentInChildren<ParticleSystem>().Play();
            
            collision.GetComponentInChildren<ParticleSystem>().Play();
            collision.GetComponent<emberScript>().SetInactive();
            StartCoroutine(impactFlash.FlashRoutine());

            dataManager.SavePlayer();
        }
        if(collision.CompareTag("Check Safe"))
        {
            player.inCheckpoint = true;
        }

        // TUTORIAL MESSAGES
        if (collision.CompareTag("TutorialCheckpoint"))
        {
            onScreenText.enabled = true;
            onScreenText.transform.position = collision.transform.position + Vector3.up * 1.6f;
            onScreenText.GetComponentsInChildren<TMP_Text>()[1].text = "Light the flame to set";
            onScreenText.GetComponentInChildren<TMP_Text>().text = "Spawn Point";
        }
        if (collision.CompareTag("TutorialJump"))
        {
            onScreenText.enabled = true;
            onScreenText.transform.position = collision.transform.position + Vector3.up * 1.3f;
            onScreenText.GetComponentsInChildren<TMP_Text>()[1].text = "Press SPACE to";
            onScreenText.GetComponentInChildren<TMP_Text>().text = "Jump";
        }
        if (collision.CompareTag("TutorialHidden"))
        {
            onScreenText.enabled = true;
            onScreenText.transform.position = collision.transform.position + Vector3.up * 1.4f;
            onScreenText.GetComponentsInChildren<TMP_Text>()[1].text = "Leap of";
            onScreenText.GetComponentInChildren<TMP_Text>().text = "Faith";
        }
        if (collision.CompareTag("TutorialMove"))
        {
            onScreenText.enabled = true;
            onScreenText.transform.position = collision.transform.position + Vector3.up * 1.6f;
            onScreenText.GetComponentsInChildren<TMP_Text>()[1].text = "Press A & D to";
            onScreenText.GetComponentInChildren<TMP_Text>().text = "Move";
        }
        if (collision.CompareTag("TutorialEmbers"))
        {
            onScreenText.enabled = true;
            onScreenText.transform.position = collision.transform.position + Vector3.up * 1.9f;
            onScreenText.GetComponentsInChildren<TMP_Text>()[1].text = "Collect resources called";
            onScreenText.GetComponentInChildren<TMP_Text>().text = "Embers";
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
            player.SetSpeedMultiplier((1.0f / 0.3f));
            if (!dataManager.data.jumpPlus)
            {
                player.SetJumpMultiplier(1.0f / 0.6f);
            }
            if (!dataManager.data.dashPlus)
            {
                player.SetDashMultiplier(1.0f / 0.6f);
            }
        }
        else if (collision.CompareTag("NPC"))
        {
            canTalk = false;
            onScreenText.enabled = false;
        }
        else if (collision.CompareTag("Altar"))
        {
            onAltar = false;
            onScreenText.enabled = false;
        }
        if (collision.CompareTag("Check Safe"))
        {
            player.inCheckpoint = false;
        }

        // TUTORIAL MESSAGES
        if (collision.CompareTag("TutorialCheckpoint"))
        {
            onScreenText.enabled = false;
            onScreenText.GetComponentsInChildren<TMP_Text>()[1].text = "Press E to";
        }
        if (collision.CompareTag("TutorialJump"))
        {
            onScreenText.enabled = false;
            onScreenText.GetComponentsInChildren<TMP_Text>()[1].text = "Press E to";
        }
        if (collision.CompareTag("TutorialHidden"))
        {
            onScreenText.enabled = false;
            onScreenText.GetComponentsInChildren<TMP_Text>()[1].text = "Press E to";
        }
        if (collision.CompareTag("TutorialMove"))
        {
            onScreenText.enabled = false;
            onScreenText.GetComponentsInChildren<TMP_Text>()[1].text = "Press E to";
        }
        if (collision.CompareTag("TutorialEmbers"))
        {
            onScreenText.enabled = false;
            onScreenText.GetComponentsInChildren<TMP_Text>()[1].text = "Press E to";
        }
    }

}
