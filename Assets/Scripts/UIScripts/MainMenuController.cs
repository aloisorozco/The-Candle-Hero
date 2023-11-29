using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public DataManager dataManager;
    public GameObject loadButton;
    public GameObject[] saveButtons;

    private void Start()
    {
        dataManager = FindAnyObjectByType<DataManager>();

        if (SaveSystem.FindSaveFile())
        {
            loadButton.SetActive(true);
        }
        else
        {
            loadButton.SetActive(false);
        }
    }
    public void PlayGame()
    {
        dataManager.data = SaveSystem.NewGame();

        Debug.Log(SaveSystem.GetFileNum());
        dataManager.data.dataFile = "Save " + SaveSystem.GetFileNum();
        SceneManager.LoadScene(dataManager.data.currentScene);
    }

    public void LoadGame()
    {
        int filesLength = SaveSystem.GetFileNum()-1;
        foreach (GameObject button in saveButtons)
        {
            
            button.SetActive(true);
            
            if(filesLength > 0)
            {
                button.GetComponent<Button>().interactable = true;
                filesLength--;
            }
            else
            {
                button.GetComponent<Button>().interactable = false;
            }
        }
        saveButtons[3].GetComponent<Button>().interactable = true;
    }

    public void DeleteSaves()
    {
        loadButton.SetActive(false);
        foreach (GameObject button in saveButtons)
        {
            button.SetActive(false);
        }
        SaveSystem.DeleteSaveFiles();
    }

    public void QuitGame()
    {
        // Quit game (won't work in unity editor -> need to build)
        Application.Quit();
    }


    public void LoadGame(string file)
    {
        dataManager.data = SaveSystem.LoadPlayer(file);
        SceneManager.LoadScene(dataManager.data.currentScene);
    }

}
