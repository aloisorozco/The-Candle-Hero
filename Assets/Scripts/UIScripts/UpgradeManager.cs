using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private DataManager dataManager;
    [SerializeField] private int livesIncrease = 2;

    void Start()
    {
        if (GameObject.Find("DataManager"))
        {
            dataManager =GameObject.Find("DataManager").GetComponent<DataManager>();
        }
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
    }

    public void OnPlumberJoeHeadlampButtonClicked()
    {
        player.GetComponent<PlayerMovement>().SetLightSetGlobalLightIntensity(2f);
    }

    public void OnPickledEyeballButtonClicked()
    {
        player.GetComponent<PlayerMovement>().SetGlobalLightIntensity(0.1f);
    }

    public void OnDashButtonClicked()
    {
        dataManager.GetComponent<DataManager>().data.dashUpgrade = true;
    }

    public void OnDoubleJumpButtonClicked()
    {
        dataManager.GetComponent<DataManager>().data.doubleJumpUpgrade = true;
    }

    public void OnWallJumpButtonClicked()
    {
        dataManager.GetComponent<DataManager>().data.wallJumpUpgrade = true;
    }
}
