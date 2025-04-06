using System.Collections;
using UnityEngine;

public class DialogueRunner : MonoBehaviour
{
    public DialogueScriptUI playerUI;
    public DialogueScriptUI girlUI;

    PlayerController player;

    public void StartDialogue(DialogueScript script, PlayerController player)
    {
        StartCoroutine(RunDialogue(script, player));
    }

    private IEnumerator RunDialogue(DialogueScript script, PlayerController player)
    {
        foreach (var line in script.lines)
        {
            if (line.speaker == DialogueLine.Speaker.Player)
                playerUI.Show(line.text);
            else
                girlUI.Show(line.text);

            yield return new WaitForSeconds(line.duration);

            playerUI.Hide();
            girlUI.Hide();
        }

        player.EndDialogue();
    }
}
