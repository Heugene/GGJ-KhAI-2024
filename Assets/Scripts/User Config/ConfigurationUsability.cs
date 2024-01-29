using UnityEngine;

public class ConfigurationUsability : MonoBehaviour
{
    private AudioSource[] allSounds;

    void Awake()
    {
        // �������� �� ����'���� �� ���� � ����������� AudioSource
        allSounds = FindObjectsOfType<AudioSource>();

        SetupMusic();
    }

    public void SetupMusic()
    {
        // ������������� ����������� �������� ��� ������
        float musicValue = PlayerPrefs.GetFloat("MusicValue");

        foreach (var track in allSounds)
        {
            track.volume = musicValue;
        }
    }

    // ������ ���������� ���������� ������ ���������������� ��������
}
