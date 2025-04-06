using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    // Этот метод можно привязать к кнопке в UI через инспектор
    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
