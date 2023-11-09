using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    [SerializeField] public string displayMessage;
    public GameObject popupCanvas;

    private void Start()
    {
        // Ensure the Canvas is initially disabled
        if (popupCanvas != null)
        {
            popupCanvas.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            popUp();
            Debug.Log("In range of Interactable");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            hidePopup();
        }
    }

    private void popUp()
    {
        if (popupCanvas != null)
        {
            popupCanvas.SetActive(true);

            popupCanvas.GetComponent<RectTransform>().position = transform.position;
            popupCanvas.transform.SetParent(transform);

            Text popupText = popupCanvas.GetComponentInChildren<Text>();
            if (popupText != null)
            {
                popupText.text = displayMessage;
            }
        }
    }

    private void hidePopup()
    {
        if (popupCanvas != null && popupCanvas.activeSelf)
        {
            popupCanvas.SetActive(false);
        }
    }
}
