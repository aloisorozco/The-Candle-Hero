
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class DataManager : MonoBehaviour
{
    public Data data;
    public RespawnPlayer rp;


    void Awake()
    {
        bool[] emberTutorial = new bool[] { false, false };
        bool[] emberLevel1 = new bool[] { false, false, false, false, false, false };
        bool[] emberLevel2 = new bool[] { false, false, false, false, false };
        bool[] emberLevel3 = new bool[] { false, false, false, false, false };
        bool[] activeLevels = new bool[] { true, false, false };
        data = new Data(0, false, false, false, "Tutorial", "InitialRespawnPoint", 5, 5, 5, "Save 1", emberTutorial, emberLevel1, emberLevel2, emberLevel3, activeLevels, false, false, false, 1);
        
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
            data.emberLevel2[emberNum - 1] = true;
        }
        else if (SceneManager.GetActiveScene().name == "Level_3")
        {
            data.emberLevel3[emberNum - 1] = true;
        }
    }

    public void BuyUpgrade(string name)
    {
        Debug.Log(name);
        if(name == "Mitochondria" || name == "Magic Mix")
        {
            data.maxLives += 2;
            data.lives = data.maxLives;
            rp.ResetHearts();
        }
        else if (name == "Eternal Flame" || name == "Pickled Eye") { data.lightRadius += 2; }
        else if(name == "Gust of Steam++") { data.dashPlus = true; }
        else if (name == "Healing Jar") { data.healthRate = 0.5f; }
        else if (name == "Lantern of Stars") { data.healthRate = 0.5f; }
        else if (name == "Spider Sense") { data.jumpPlus = true; }
        else if (name == "Flame Step++") { data.doubleJumpPlus = true; }
        
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

    public void SetActiveScenes(int i)
    {
        Debug.Log("DataManager: Line 73: SetActiveScene(int i): " + i);
        data.activeLevels[i] = true;
    }

    public bool GetActiveLevel(int i)
    {
        return data.activeLevels[i];
    }


}
