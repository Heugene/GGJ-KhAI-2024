using UnityEngine;
using UnityEngine.UI;

public class ConfigurationCreator : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    private float previousMusicValue; // ��� �������� ����������� �������� ��������

    [SerializeField] Slider soundSlider;
    private float previousSoundValue; // ��� �������� ����������� �������� ��������

    private void Awake()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicValue");
        soundSlider.value = PlayerPrefs.GetFloat("SoundValue");

        // ���������� ����������� ��������� �������� ��������
        musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
        soundSlider.onValueChanged.AddListener(OnSoundSliderValueChanged);
    }

    private void OnMusicSliderValueChanged(float value)
    {
        // ���������� �������� ������ ��� ����������� ���������
        if (value != previousMusicValue)
        {
            PlayerPrefs.SetFloat("MusicValue", value);
            previousMusicValue = value;

            transform.GetComponent<ConfigurationUsability>().SetupMusic();
        }
    }

    private void OnSoundSliderValueChanged(float value)
    {
        // ���������� �������� ������ ��� ����������� ���������
        if (value != previousSoundValue)
        {
            PlayerPrefs.SetFloat("SoundValue", value);
            previousSoundValue = value;

            transform.GetComponent<ConfigurationUsability>().SetupSounds();
        }
    }
}
