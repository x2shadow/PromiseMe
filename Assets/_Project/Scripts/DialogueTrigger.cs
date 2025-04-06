using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueScript dialogueScript;   // Сценарий диалога
    public DialogueRunner dialogueRunner;   // Ссылка на DialogueRunner

    private bool hasTriggered = false;      // Чтобы не запускать повторно

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        if (dialogueRunner != null && dialogueScript != null)
        {
            dialogueRunner.StartDialogue(dialogueScript);
        }
        else
        {
            Debug.LogWarning("DialogueRunner или DialogueScript не назначены в инспекторе.");
        }
    }
}
