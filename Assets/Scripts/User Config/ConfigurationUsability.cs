using System.Collections.Generic;
using UnityEngine;

public class ConfigurationUsability : MonoBehaviour
{
    [SerializeField] List<AudioSource> tracks;
    [SerializeField] List<AudioSource> sounds;
    [SerializeField] List<GameObject> prefabs;

    void Awake()
    {
        SetupMusic();
        SetupSounds();
    }

    public void SetupMusic()
    {
        // ������������� ����������� �������� ��� ������
        float musicValue = PlayerPrefs.GetFloat("MusicValue");

        foreach (var track in tracks)
        {
            track.volume = musicValue;
        }
    }

    // ������ ���������� ���������� ������ ���������������� ��������

    public void SetupSounds()
    {
        // ������������� ����������� �������� ��� ������
        float soundValue = PlayerPrefs.GetFloat("SoundValue");

        foreach (var prefab in prefabs)
        {
            sounds.Add(prefab.GetComponent<AudioSource>());
        }

        Debug.Log("Sounds volume:" + soundValue);
        foreach (var sound in sounds)
        {
            sound.volume = soundValue;
            Debug.Log("Sound value assigned");
        }
    }
}
