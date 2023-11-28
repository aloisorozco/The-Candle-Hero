
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public Data data;


    void Awake()
    {
        data = new Data(0, false, false, false, "Tutorial", "InitialRespawnPoint", 3, 5);
        
        DontDestroyOnLoad(this.gameObject);
        
    }

    public void AddEmber()
    {
        data.embers++;
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
        SaveSystem.SavePlayer(data);
    }


}
