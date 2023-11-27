using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//source: https://www.youtube.com/watch?v=vY0Sk93YUhA&ab_channel=ShapedbyRainStudios

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Dialogue Manager")]
    [SerializeField] private GameObject dialogueManager;

    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && !dialogueManager.GetComponent<DialogueManager>().dialogueIsPlaying)
        {
            visualCue.SetActive(true);
            if (Input.GetMouseButtonDown(0))
            {
                dialogueManager.GetComponent<DialogueManager>().EnterDialogueMode(inkJSON);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
