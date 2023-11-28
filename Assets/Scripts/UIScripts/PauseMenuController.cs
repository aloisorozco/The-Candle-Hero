using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenuController : MonoBehaviour
{
    // Check if game is paused
    // Accesible from other scripts
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Time.timeScale = 0f;
                Pause();
            }
        }
    }

    public void Resume()
    {
        // Hide Menu
        pauseMenuUI.SetActive(false);

        // Continue game time
        Time.timeScale = 1f;

        GameIsPaused = false;
    }

    void Pause()
    {
        // Show Menu
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);

        // Stop game from running
        

        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        // Won't do anything in unity editor. Debug is there to checkk functionality.
        Debug.Log("Quit");
        Application.Quit();
    }
}
