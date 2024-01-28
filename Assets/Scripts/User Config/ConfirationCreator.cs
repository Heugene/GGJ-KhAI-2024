using UnityEngine;
using UnityEngine.UI;

public class ConfigurationCreator : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    private float previousMusicValue; // Для хранения предыдущего значения слайдера

    [SerializeField] Slider soundSlider;
    private float previousSoundValue; // Для хранения предыдущего значения слайдера

    private void Awake()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicValue");
        soundSlider.value = PlayerPrefs.GetFloat("SoundValue");

        // Добавление обработчика изменения значения слайдера
        musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
        soundSlider.onValueChanged.AddListener(OnSoundSliderValueChanged);
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

    private void OnSoundSliderValueChanged(float value)
    {
        // Сохранение значения только при фактическом изменении
        if (value != previousSoundValue)
        {
            PlayerPrefs.SetFloat("SoundValue", value);
            previousSoundValue = value;

            transform.GetComponent<ConfigurationUsability>().SetupSounds();
        }
    }
}
