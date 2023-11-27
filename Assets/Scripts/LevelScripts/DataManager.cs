using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public Data data;


    void Awake()
    {
        data = new Data(0, false, false, false, "Tutorial", "InitialRespawnPoint");
        
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
        Debug.Log(respawnPoint);
        data.respawnPoint = respawnPoint;
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(data);
    }


}
