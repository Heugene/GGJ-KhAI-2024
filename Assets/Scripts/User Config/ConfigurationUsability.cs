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
        // Инициализация предыдущего значения при старте
        float musicValue = PlayerPrefs.GetFloat("MusicValue");

        foreach (var track in tracks)
        {
            track.volume = musicValue;
        }
    }

    // Дальше добавление применений других пользовательских настроек

    public void SetupSounds()
    {
        // Инициализация предыдущего значения при старте
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
