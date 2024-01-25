using UnityEngine;
using UnityEngine.UI;

public class ConfigurationCreator : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    private float previousMusicValue; // ��� �������� ����������� �������� ��������

    private void Awake()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicValue");

        // ���������� ����������� ��������� �������� ��������
        musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
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
}
