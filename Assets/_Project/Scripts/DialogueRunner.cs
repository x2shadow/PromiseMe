using System.Collections;
using UnityEngine;

public class DialogueRunner : MonoBehaviour
{
    public DialogueScriptUI playerUI;
    public DialogueScriptUI girlUI;

    public void StartDialogue(DialogueScript script)
    {
        StartCoroutine(RunDialogue(script));
    }

    private IEnumerator RunDialogue(DialogueScript script)
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
    }
}
