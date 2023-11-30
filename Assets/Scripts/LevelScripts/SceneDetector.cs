using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDetector : MonoBehaviour
{
    public int sceneNumber;
    private DataManager dataManager;
    [SerializeField] LevelTeleport levelTeleport;

    private void Start()
    {
        dataManager = FindAnyObjectByType<DataManager>();
        dataManager.SetActiveScenes(sceneNumber);
        levelTeleport.SetActiveLevelButtons();
        Debug.Log("Scene Detector: sceneNumber: " + sceneNumber);
    }
}
