using UnityEngine;
using UnityEngine.UI;

public class EnergySliderController : MonoBehaviour
{
    [SerializeField] private GameObject energySlider; // объект, который будем включать/выключать
    [SerializeField] private Toggle energyToggle; // Toggle из меню

    private const string EnergyToggleKey = "ShowEnergySlider";

    private void Start()
    {
        // Загружаем сохраненное значение
        bool showSlider = PlayerPrefs.GetInt(EnergyToggleKey, 0) == 1;
        if (energySlider != null) energySlider.SetActive(showSlider);
        energyToggle.isOn = showSlider;

        energyToggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool isOn)
    {
        if (energySlider != null) energySlider.SetActive(isOn);

        PlayerPrefs.SetInt(EnergyToggleKey, isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}
