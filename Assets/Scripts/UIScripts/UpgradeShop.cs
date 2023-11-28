using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeShop : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] ResourceManager candleCount;
    [SerializeField] Canvas upgradeShopCanvas;

    // Update is called once per frame
    void Update()
    {
        coinText.text = "Candles: " + candleCount.currentCandles;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitButton();
        }
    }

    public void ExitButton()
    {
        Time.timeScale = 1f;
        upgradeShopCanvas.enabled = false;
    }
}
