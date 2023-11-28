using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject dataManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMitochondriaButtonClicked()
    {

    }

    public void OnStrongGreensButtonClicked()
    {

    }

    public void OnHealingEmberButtonClicked()
    {

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
