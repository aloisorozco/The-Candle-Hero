using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    public void AddEmber()
    {
        dataManager.AddEmber();
        currentEmbers = dataManager.data.embers;
        SetCountCandle(currentEmbers);
    }

    public void SetCountCandle(int count)
    {
        currentEmbers = count;
        candleCountText.text = currentEmbers.ToString();
    }

}
