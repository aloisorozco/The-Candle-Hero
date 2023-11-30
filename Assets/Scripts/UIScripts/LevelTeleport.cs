using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelTeleport : MonoBehaviour
{
    private DataManager dataManager;
    public Button[] levelButtons;

    public void SetActiveLevelButtons()
    {
        dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
        for (int i = 0; i <levelButtons.Length; i++) {
            Debug.Log("Index: LevelTeleport line 19: " + i);
            if (dataManager.GetActiveLevel(i) == false)
            {
                levelButtons[i].interactable = false;
                levelButtons[i].image.color = Color.black;
            }
            else
            {
                levelButtons[i].interactable = true;
                levelButtons[i].image.color = Color.white;
            }  
        }
    }

    public void ChangeLevels(int i)
    {
        // Scene numbers are 1-3, array is 0-2
        Time.timeScale = 1f;
        dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
        if (dataManager.GetActiveLevel(i - 1) == true && SceneManager.GetActiveScene().buildIndex != i)
            dataManager.data.respawnPoint = "InitialRespawnPoint";
            SceneManager.LoadScene(i);
    }
}
