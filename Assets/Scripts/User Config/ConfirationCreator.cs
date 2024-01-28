using UnityEngine;
using UnityEngine.UI;

public class ConfigurationCreator : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    private float previousMusicValue; // Для хранения предыдущего значения слайдера

    private void Awake()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicValue");

        // Добавление обработчика изменения значения слайдера
        musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
    }

    private void OnMusicSliderValueChanged(float value)
    {
        // Сохранение значения только при фактическом изменении
        if (value != previousMusicValue)
        {
            PlayerPrefs.SetFloat("MusicValue", value);
            previousMusicValue = value;

            transform.GetComponent<ConfigurationUsability>().SetupMusic();
        }
    }
}
