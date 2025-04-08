using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonWithoutPause : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    public void OnMainMenuButtonClicked()
    {
        Time.timeScale = 1f;                       // Возвращаем время
        playerController.SetInputBlocked(false); // Возвращаем управление
        SceneManager.LoadScene("MainMenu");
    }
}
