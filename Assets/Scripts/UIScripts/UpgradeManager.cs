using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private DataManager dataManager;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private int livesIncrease = 2;

    [Header("Abilities")]
    [SerializeField] private Canvas abilityPopUp;
    [SerializeField] private TMP_Text abilityNameText;
    [SerializeField] private TMP_Text abilityDescriptionText;

    void Start()
    {
        if (GameObject.Find("DataManager"))
        {
            dataManager =GameObject.Find("DataManager").GetComponent<DataManager>();
        }
        abilityPopUp.enabled = false;
    }

    public void OnMitochondriaButtonClicked()
    {
        player.GetComponent<PlayerMovement>().AddLife(livesIncrease);
        dataManager.data.lives += livesIncrease;
    }

    public void OnStrongGreensButtonClicked()
    {
        player.GetComponent<PlayerMovement>().AddLife(livesIncrease);
        dataManager.data.lives += livesIncrease;
    }

    public void OnHealingEmberButtonClicked()
    {
        player.GetComponent<PlayerMovement>().AddHealingEmber();
    }

    public void OnEternalFlameButtonClicked()
    {
        player.GetComponent<PlayerMovement>().SetMaxLightRadius(8f);
        dataManager.data.lightRadius = 8;
    }

    public void OnPlumberJoeHeadlampButtonClicked()
    {
        player.GetComponent<PlayerMovement>().SetLightSetGlobalLightIntensity(2f);
    }

    public void OnPickledEyeballButtonClicked()
    {
        player.GetComponent<PlayerMovement>().SetGlobalLightIntensity(0.1f);
    }



    public void openAbilityPopUp(string name, string description)
    {
        abilityPopUp.enabled = true;
        abilityNameText.text = name;
        abilityDescriptionText.text = description;
    }

    public void closeAbilityPopUp()
    {
        abilityPopUp.enabled = false;
        dialogueManager.dialogueIsPlaying = false;
    }
}
