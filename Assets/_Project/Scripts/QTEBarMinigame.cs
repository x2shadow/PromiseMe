using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QTEBarMinigame : MonoBehaviour
{
    public float fillSpeed = 0.1f;
    public float drainSpeed = 0.5f;
    public Slider qteSlider;
    public PlayerController playerController;
    public GirlFollower girl;

    private float progress = 0f;
    private bool isPlaying = false;

    private InputAction spamAction;

    public void StartQTE(InputAction action)
    {
        if (isPlaying) return;

        spamAction = action;
        spamAction.performed += OnSpamPressed;

        isPlaying = true;
        progress = 0f;

        qteSlider.gameObject.SetActive(true);
        qteSlider.value = 0f;

        playerController?.SetInputBlocked(true);
    }

    private void OnSpamPressed(InputAction.CallbackContext context)
    {
        if (!isPlaying) return;

        progress += fillSpeed; // убираем Time.deltaTime
        progress = Mathf.Clamp01(progress);
        qteSlider.value = progress;

        if (progress >= 1f)
        {
            CompleteQTE();
        }
    }

    private void Update()
    {
        if (!isPlaying) return;

        // Плавное опустошение
        progress -= drainSpeed * Time.deltaTime;
        progress = Mathf.Clamp01(progress);
        qteSlider.value = progress;
    }

    private void CompleteQTE()
    {
        isPlaying = false;

        spamAction.performed -= OnSpamPressed;

        qteSlider.gameObject.SetActive(false);
        playerController?.SetInputBlocked(false);

        Debug.Log("QTE завершено успешно!");

        girl.isDead = true;
    }

    private void OnDisable()
    {
        if (spamAction != null)
        {
            spamAction.performed -= OnSpamPressed;
        }
    }
}
