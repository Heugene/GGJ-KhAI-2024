using UnityEngine;

public class ConfigurationUsability : MonoBehaviour
{
    private AudioSource[] allSounds;

    void Awake()
    {
        // Отримуємо всі гриб'єкти на сцені з компонентом AudioSource
        allSounds = FindObjectsOfType<AudioSource>();

        SetupMusic();
    }

    public void SetupMusic()
    {
        // Инициализация предыдущего значения при старте
        float musicValue = PlayerPrefs.GetFloat("MusicValue");

        foreach (var track in allSounds)
        {
            track.volume = musicValue;
        }
    }

    // Дальше добавление применений других пользовательских настроек
}
