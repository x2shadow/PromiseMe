using UnityEngine;

public class EnergySliderInitializer : MonoBehaviour
{
    [SerializeField] private GameObject energySlider;
    private const string EnergyToggleKey = "ShowEnergySlider";

    private void Start()
    {
        bool showSlider = PlayerPrefs.GetInt(EnergyToggleKey, 0) == 1;
        energySlider.SetActive(showSlider);
    }
}
