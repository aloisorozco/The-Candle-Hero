using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResourceManager : MonoBehaviour
{
    public int currentEmbers = 0;
    public TMP_Text candleCountText;
    public GameObject dataManagerPrefab;

    public DataManager dataManager;


    void Awake()
    {
        candleCountText.text = currentEmbers.ToString();

        if (FindAnyObjectByType<DataManager>())
        {
            dataManager = FindAnyObjectByType<DataManager>();
            currentEmbers = dataManager.data.embers;
        }
        else
        {
            GameObject dataM = Instantiate(dataManagerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            dataManager = dataM.GetComponent<DataManager>();
        }
        SetCountCandle(currentEmbers);
    }

    private void Start()
    {
        SetActiveEmbers();
    }

    private void Update()
    {
        currentEmbers = dataManager.data.embers;
    }

    public void AddEmber(int num)
    {
        dataManager.AddEmber(num);
        currentEmbers = dataManager.data.embers;
        SetCountCandle(currentEmbers);
    }

    public void SetCountCandle(int count)
    {
        currentEmbers = count;
        candleCountText.text = currentEmbers.ToString();
    }

    public void SetActiveEmbers()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        GameObject embers = GameObject.Find("Embers");
        int numEmbers = 0;
        bool[] activeEmbers;

        if (currentScene == "Tutorial") { activeEmbers = dataManager.data.emberTutorial; }
        else if (currentScene == "Level_1") { activeEmbers = dataManager.data.emberLevel1; }
        else if (currentScene == "Level_2") { activeEmbers = dataManager.data.emberLevel2; }
        else if (currentScene == "Level_3") { activeEmbers = dataManager.data.emberLevel3; }
        else { return; }

        foreach (Transform child in embers.transform)
        {
            if (activeEmbers[numEmbers])
            {
                Debug.Log(child.name + " Set Inactive");
                child.GetComponent<emberScript>().SetInactive();
            }
            numEmbers++;
            
        }
    }

}
