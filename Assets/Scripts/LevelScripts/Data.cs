using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

[System.Serializable]
public class Data
{
    public int embers;
    public bool dashUpgrade;
    public bool doubleJumpUpgrade;
    public bool wallJumpUpgrade;

    public string currentScene;
    public string respawnPoint;

    public Data()
    {
        this.embers = 0;
        this.dashUpgrade = false;
        this.doubleJumpUpgrade = false;
        this.wallJumpUpgrade = false;
        this.currentScene = "Tutorial";
        this.respawnPoint = "InitialRespawnPoint";
    }
    public Data(int embers, bool dashUpgrade, bool doubleJumpUpgrade, bool wallJumpUpgrade, string currentScene, string respawnPoint)
    {
        this.embers = embers;
        this.dashUpgrade = dashUpgrade;
        this.doubleJumpUpgrade = doubleJumpUpgrade;
        this.wallJumpUpgrade = wallJumpUpgrade;
        this.currentScene = currentScene;
        this.respawnPoint = respawnPoint;
    }
}
