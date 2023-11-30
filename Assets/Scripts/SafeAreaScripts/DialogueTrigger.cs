using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//source: https://www.youtube.com/watch?v=vY0Sk93YUhA&ab_channel=ShapedbyRainStudios

public class DialogueTrigger : MonoBehaviour
{
    [Header("Name")]
    [SerializeField] private string npcName;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private TextAsset inkJSONRepeat;

    [Header("Dialogue Manager")]
    [SerializeField] private GameObject dialogueManager;

    private bool hasTalkedToBefore = false;
    private CollisionDetection collisionDetector;

    private void Start()
    {
        collisionDetector = FindAnyObjectByType<CollisionDetection>();
    }

    private void Update()
    {
        if (collisionDetector.canTalk && !dialogueManager.GetComponent<DialogueManager>().dialogueIsPlaying)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!hasTalkedToBefore)
                {
                    hasTalkedToBefore = true;
                    dialogueManager.GetComponent<DialogueManager>().EnterDialogueMode(inkJSON, npcName);
                }
                else
                {
                    dialogueManager.GetComponent<DialogueManager>().EnterDialogueMode(inkJSONRepeat, npcName);
                }
            }
        }

        if (!collisionDetector.canTalk && dialogueManager.GetComponent<DialogueManager>().dialogueIsPlaying)
        {
            StartCoroutine(dialogueManager.GetComponent<DialogueManager>().ExitDialogueMode());
        }
    }
}
