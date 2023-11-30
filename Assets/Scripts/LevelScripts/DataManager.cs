
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public Data data;


    void Awake()
    {
        bool[] emberTutorial = new bool[] { false, false, false };
        bool[] emberLevel1 = new bool[] { false, false, false, false, false, false };
        bool[] emberLevel2 = new bool[] { false, false, false, false, false };
        bool[] emberLevel3 = new bool[] { false, false, false, false, false };
        data = new Data(0, false, false, false, "Tutorial", "InitialRespawnPoint", 5, 5, 5, "Save 1", emberTutorial, emberLevel1, emberLevel2, emberLevel3);
        
        DontDestroyOnLoad(this.gameObject);
        
    }

    

    public void AddEmber(int emberNum)
    {
        data.embers++;
        if(SceneManager.GetActiveScene().name == "Tutorial")
        {
            data.emberTutorial[emberNum - 1] = true;
        }
        else if (SceneManager.GetActiveScene().name == "Level_1")
        {
            data.emberLevel1[emberNum - 1] = true;
        }
        else if (SceneManager.GetActiveScene().name == "Level_2")
        {
            data.emberLevel1[emberNum - 1] = true;
        }
        else if (SceneManager.GetActiveScene().name == "Level_3")
        {
            data.emberLevel1[emberNum - 1] = true;
        }
    }

    public void ResetLives()
    {
        data.lives = data.maxLives;
    }

    public void SetDashUpgrade()
    {
        data.dashUpgrade = true;
    }

    public void SetDoubleJumpUpgrade()
    {
        data.doubleJumpUpgrade = true;
    }
    public void SetWallJumpUpgrade()
    {
        data.wallJumpUpgrade = true;
    }

    public void SetRespawnPoint(string respawnPoint)
    {
        data.respawnPoint = respawnPoint;
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(data, data.dataFile);
    }


}
