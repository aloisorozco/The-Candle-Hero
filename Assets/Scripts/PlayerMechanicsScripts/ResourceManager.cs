using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public int currentCandles = 0;
    public TMP_Text candleCountText;

    public DataManager dataManager;

    void Awake()
    {
        candleCountText.text = currentCandles.ToString();

        if(GameObject.Find("DataManager") != null)
        {
            dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
            SetCountCandle(dataManager.resourceManager.currentCandles);
        }
        else
        {
            SetCountCandle(currentCandles);
        }

    }

    public void AddCountCandle(int count)
    {
        currentCandles+= count;
        candleCountText.text = currentCandles.ToString();
    }

    public void SetCountCandle(int count)
    {
        currentCandles = count;
        candleCountText.text = currentCandles.ToString();
    }

}
