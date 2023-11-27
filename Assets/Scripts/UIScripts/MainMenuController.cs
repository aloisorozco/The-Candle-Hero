using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    public DataManager dataManager;

    private void Start()
    {
        dataManager = FindAnyObjectByType<DataManager>();
    }
    public void PlayGame()
    {
        dataManager.data = SaveSystem.LoadPlayer();
        //Play the next scene in queue
        SceneManager.LoadScene(dataManager.data.currentScene);
    }

    public void QuitGame()
    {
        // Quit game (won't work in unity editor -> need to build)
        Application.Quit();
    }
}
