using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public GameObject playerPanel;
    public TMP_Text playerText;

    public GameObject girlPanel;
    public TMP_Text girlText;

    public GameObject tutorialPanel;
    public TMP_Text tutorialText;

    public float dialogueDuration = 3f;

    public void ShowPlayerDialogue(string text)
    {
        StartCoroutine(ShowDialogue(playerPanel, playerText, text));
    }

    public void ShowGirlDialogue(string text)
    {
        StartCoroutine(ShowDialogue(girlPanel, girlText, text));
    }

    public void ShowTutorialDialogue(string text)
    {
        StartCoroutine(ShowDialogue(tutorialPanel, tutorialText, text));
    }

    private IEnumerator ShowDialogue(GameObject panel, TMP_Text textComponent, string text)
    {
        panel.SetActive(true);
        textComponent.text = text;
        yield return new WaitForSeconds(dialogueDuration);
        panel.SetActive(false);
    }
}
