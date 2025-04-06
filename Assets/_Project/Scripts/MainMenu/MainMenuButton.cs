using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
