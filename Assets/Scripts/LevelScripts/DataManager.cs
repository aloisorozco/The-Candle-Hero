
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
        bool[] activeLevels = new bool[] { true, false, false, false};
        bool[] boughtUpgrades = new bool[] { false, false, false, false, false, false, false, false, false, false, false, false };
        data = new Data(0, false, false, false, "Tutorial", "InitialRespawnPoint", 5, 5, 5, "Save 1", emberTutorial, emberLevel1, emberLevel2, emberLevel3, activeLevels, false, false, false, 1, boughtUpgrades);
        
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

    public void RemoveEmber(int amount)
    {
        data.embers -= amount;
    }

    public void BuyUpgrade(string name)
    {
        rp = GameObject.Find("DeathBox").GetComponent<RespawnPlayer>();
        if(name == "Mitochondria")
        {
            data.maxLives += 2;
            data.lives = data.maxLives;
            rp.ResetHearts();
            data.boughtUpgrades[6] = true;
        }
        else if (name == "Eternal Flame") { 
            data.lightRadius += 2;
            data.boughtUpgrades[3] = true;
        }
        else if(name == "Gust of Steam++") { data.dashPlus = true; data.boughtUpgrades[9] = true; }
        else if (name == "Healing Jar") { data.healthRate = 0.5f; data.boughtUpgrades[7] = true; }
        else if (name == "Lantern of Stars") { data.healthRate = 0.5f; data.boughtUpgrades[4] = true; }
        else if (name == "Spider Sense") { data.jumpPlus = true; data.boughtUpgrades[10] = true; }
        else if (name == "Flame Step++") { data.doubleJumpPlus = true; data.boughtUpgrades[11] = true; }
        else if (name == "Magic Mix") {
            data.maxLives += 2;
            data.lives = data.maxLives;
            rp.ResetHearts();
            data.boughtUpgrades[8] = true;
        }
        else if (name == "Pickled Eye") { data.lightRadius += 2; data.boughtUpgrades[5] = true; }

    }

    public void UpdateUI()
    {
        if(GameObject.Find("Inventory Upgrade Images"))
        {
            GameObject images = GameObject.Find("Inventory Upgrade Images");
            int count = 0;
            foreach(Transform child in images.transform)
            {
                if (data.boughtUpgrades[count])
                {
                    child.gameObject.SetActive(true);
                }
                count++;
            }
        }
    }

    public void ResetLives()
    {
        data.lives = data.maxLives;
    }

    public void SetDashUpgrade()
    {
        data.boughtUpgrades[0] = true;
        data.dashUpgrade = true;
    }

    public void SetDoubleJumpUpgrade()
    {
        data.boughtUpgrades[2] = true;
        data.doubleJumpUpgrade = true;
    }
    public void SetWallJumpUpgrade()
    {
        data.boughtUpgrades[1] = true;
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
        data.activeLevels[i] = true;
    }

    public bool GetActiveLevel(int i)
    {
        return data.activeLevels[i];
    }


}
