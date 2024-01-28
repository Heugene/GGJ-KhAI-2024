using System.Collections.Generic;
using UnityEngine;

public class ConfigurationUsability : MonoBehaviour
{
    [SerializeField] List<AudioSource> tracks;

    void Awake()
    {
        SetupMusic();
    }

    public void SetupMusic()
    {
        // »нициализаци€ предыдущего значени€ при старте
        float musicValue = PlayerPrefs.GetFloat("MusicValue");

        foreach (var track in tracks)
        {
            track.volume = musicValue;
        }
    }

    // ƒальше добавление применений других пользовательских настроек
}
