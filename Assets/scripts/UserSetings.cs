using UnityEngine;
using UnityEngine.UI;

public class UserSetings : MonoBehaviour
{
    public static float musicValue;

    [SerializeField] Slider musicSlider;
    [SerializeField] AudioSource audio; // TODO: �������� �� ������������� ���������� ������

    private float previousMusicValue; // ��� �������� ����������� �������� ��������

    private void Awake()
    {
        // ������������� ����������� �������� ��� ������
        previousMusicValue = PlayerPrefs.GetFloat("MusicValue");
        musicSlider.value = previousMusicValue;
        audio.volume = previousMusicValue;
    }

    private void Start()
    {
        // ���������� ����������� ��������� �������� ��������
        musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
    }

    private void OnMusicSliderValueChanged(float value)
    {
        musicValue = value;

        // ���������� �������� ������ ��� ����������� ���������
        if (musicValue != previousMusicValue)
        {
            PlayerPrefs.SetFloat("MusicValue", musicValue);
            previousMusicValue = musicValue;
        }

        audio.volume = musicValue;
    }
}
