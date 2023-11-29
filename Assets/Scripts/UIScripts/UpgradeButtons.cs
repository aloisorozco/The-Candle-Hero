using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonHandler : MonoBehaviour
{
    [Header("UI GameObjects")]
    [SerializeField] GameObject confirmationUI;
    [SerializeField] GameObject inventoryUI;
    [SerializeField] TextMeshProUGUI confirmText;
    [SerializeField] GameObject confirmButton;
    [SerializeField] GameObject cancelButton;

    [Header("Upgrade Information")]
    public int numUpdates = 0;
    [SerializeField] GameObject[] prefabs;
    [SerializeField] int[] prices;
    [SerializeField] TextMeshProUGUI[] pricesText;


    [SerializeField] ResourceManager playerResource;
    private void Start()
    {
        for(int i = 0; i < prices.Length; i++)
        {
            pricesText[i].SetText(prices[i].ToString());
        }
    }

    public void OnButtonClick(int i)
    {
        // Show the confirmation UI
        if (prices[i] > playerResource.currentEmbers)
        {
            confirmationUI.SetActive(true);
            confirmText.text = "You do not have enough to purchase this";
            confirmButton.SetActive(false);
        }
        else
        {
            confirmationUI.SetActive(true);
            confirmButton.SetActive(true);
            numUpdates = i;
            confirmText.text = "Are you sure you want to purchase this?";   
        }
    }

    public void OnConfirmButtonClick()
    {
        // Instantiate the upgrade prefab
        if (numUpdates >= 0 && numUpdates <= 2) { 
            GameObject newUpgrade = Instantiate(prefabs[numUpdates], new Vector3(1000, 610 - (numUpdates * 161), 0), Quaternion.identity, inventoryUI.transform);
            playerResource.SetCountCandle(playerResource.currentEmbers - prices[numUpdates]);
        }

        // Close the confirmation UI
        confirmationUI.SetActive(false);
    }

    public void OnCancelButtonClick()
    {
        // Close the confirmation UI without taking any action
        confirmationUI.SetActive(false);
    }
}
