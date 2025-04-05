using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BreathingMinigame : MonoBehaviour
{
    public int requiredCycles = 3;
    public float cooldownTime = 1.0f;
    public float inhaleSpeed = 1.0f; // скорость наполнения полоски
    public Slider breathSlider; // ссылка на слайдер

    private int currentCycle = 0;
    private float cooldownTimer = 0f;
    private bool isPlaying = false;

    private enum State { Idle, Inhaling, Cooldown }
    private State state = State.Idle;

    private InputAction breathAction;
    private float inhaleProgress = 0f;

    public PlayerController playerController;

    public void StartMinigame(InputAction action)
    {
        if (isPlaying) return;


        breathAction = action;
        breathAction.performed += OnBreathStarted;
        breathAction.canceled += OnBreathEnded;

        Debug.Log("Дыхательная мини-игра началась!");
        playerController?.SetInputBlocked(true);
        isPlaying = true;
        currentCycle = 0;
        inhaleProgress = 0f;
        state = State.Idle;

        if (breathSlider != null)
        {
            breathSlider.gameObject.SetActive(true);
            breathSlider.value = 0f;
        }
    }

    private void OnBreathStarted(InputAction.CallbackContext context)
    {
        if (!isPlaying || state != State.Idle) return;

        Debug.Log("Вдох");
        state = State.Inhaling;
    }

    private void OnBreathEnded(InputAction.CallbackContext context)
    {
        if (!isPlaying || state != State.Inhaling) return;

        Debug.Log("Выдох");
        currentCycle++;

        if (currentCycle >= requiredCycles)
        {
            Debug.Log("Мини-игра завершена");
            EndMinigame();
            return;
        }

        state = State.Cooldown;
        cooldownTimer = cooldownTime;
        inhaleProgress = 0f;
        if (breathSlider != null)
        {
            breathSlider.value = 0f;
        }
    }

    private void Update()
    {
        if (!isPlaying) return;

        if (state == State.Inhaling)
        {
            inhaleProgress += inhaleSpeed * Time.deltaTime;
            inhaleProgress = Mathf.Clamp01(inhaleProgress);
            if (breathSlider != null)
                breathSlider.value = inhaleProgress;
        }

        if (state == State.Cooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                state = State.Idle;
            }
        }
    }

    private void EndMinigame()
    {
        isPlaying = false;
        state = State.Idle;

        if (breathAction != null)
        {
            breathAction.performed -= OnBreathStarted;
            breathAction.canceled -= OnBreathEnded;
        }

        if (breathSlider != null)
        {
            breathSlider.gameObject.SetActive(false);
            breathSlider.value = 0f;
        }

        Debug.Log("Дыхательная мини-игра завершена полностью.");
        playerController?.SetInputBlocked(false);
    }

    private void OnDisable()
    {
        if (breathAction != null)
        {
            breathAction.performed -= OnBreathStarted;
            breathAction.canceled -= OnBreathEnded;
        }
    }
}
