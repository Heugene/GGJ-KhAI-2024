using UnityEngine;
using UnityEngine.UI;

public class UserSetings : MonoBehaviour
{
    public static float musicValue;

    [SerializeField] Slider musicSlider;
    [SerializeField] AudioSource audio; // TODO: Изменить на использование нескольких треков

    private float previousMusicValue; // Для хранения предыдущего значения слайдера

    private void Awake()
    {
        // Инициализация предыдущего значения при старте
        previousMusicValue = PlayerPrefs.GetFloat("MusicValue");
        musicSlider.value = previousMusicValue;
        audio.volume = previousMusicValue;
    }

    private void Start()
    {
        // Добавление обработчика изменения значения слайдера
        musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
    }

    private void OnMusicSliderValueChanged(float value)
    {
        musicValue = value;

        // Сохранение значения только при фактическом изменении
        if (musicValue != previousMusicValue)
        {
            PlayerPrefs.SetFloat("MusicValue", musicValue);
            previousMusicValue = musicValue;
        }

        audio.volume = musicValue;
    }
}
