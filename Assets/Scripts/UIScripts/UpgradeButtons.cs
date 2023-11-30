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

    private string upgradeSelected;
    private int costSelected;
    private DataManager dm;
    private ResourceManager rm;


    [SerializeField] ResourceManager playerResource;
    private void Start()
    {
        dm = GameObject.Find("DataManager").GetComponent<DataManager>();
        rm = GameObject.Find("Player").GetComponent<ResourceManager>();
        for (int i = 0; i < prices.Length; i++)
        {
            pricesText[i].SetText(prices[i].ToString());
        }
    }

    public void OnButtonClick(int i)
    {
        upgradeSelected = GameObject.Find("Upgrade Title" +  (i+1)).GetComponent<TMP_Text>().text;
        costSelected = i;
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


        dm.BuyUpgrade(upgradeSelected);
        dm.UpdateUI();
        rm.RemoveEmber(prices[costSelected]);

        // Close the confirmation UI
        confirmationUI.SetActive(false);
    }

    public void OnCancelButtonClick()
    {
        // Close the confirmation UI without taking any action
        confirmationUI.SetActive(false);
    }
}
