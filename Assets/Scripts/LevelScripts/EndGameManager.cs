using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    private DataManager dataManager;

    [Header("End UI")]
    [SerializeField] private Canvas endGameCanvas;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text nbEmbersText;
    [SerializeField] private TMP_Text nbUpgradesText;

    // Start is called before the first frame update
    void Start()
    {
        dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void displayEndGameUI()
    {
        StopCoroutine(dataManager.GameTime());
        endGameCanvas.enabled = true;

        UpdateTimerText(dataManager.data.gameTimer);
        displayNbEmbers();
    }

    private void UpdateTimerText(float seconds)
    {
        // Format seconds into minutes:seconds
        string minutes = Mathf.Floor(seconds / 60).ToString("00");
        string secondsString = (seconds % 60).ToString("00");

        timeText.text = $"{minutes}:{secondsString}";
    }

    private void displayNbEmbers()
    {
        int nbEmbers = 0;

        foreach (bool ember in dataManager.data.emberTutorial)
        {
            if (ember)
            {
                nbEmbers++;
            }
        }
        foreach (bool ember in dataManager.data.emberLevel1)
        {
            if (ember)
            {
                nbEmbers++;
            }
        }
        foreach (bool ember in dataManager.data.emberLevel2)
        {
            if (ember)
            {
                nbEmbers++;
            }
        }
        foreach (bool ember in dataManager.data.emberLevel3)
        {
            if (ember)
            {
                nbEmbers++;
            }
        }


        nbEmbersText.text = nbEmbers.ToString() + "/14";
    }

    private void displayNbUpgrades()
    {
        //for (int i = 0; i < dataManager.data.upgrades)
        //{

        //}
    }
}
