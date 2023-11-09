using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private int currentCandles = 0;
    public TMP_Text candleCountText;
    void Start()
    {
        candleCountText.text = currentCandles.ToString();
    }

    public void AddCountCandle(int count)
    {
        currentCandles+= count;
        candleCountText.text = currentCandles.ToString();
    }

}
