using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressLogic : MonoBehaviour
{
    internal bool isPressed { get; private set; } = false; // „и натиснута кнопка
    private Animator anim;
    public AudioClip pressedSound;
    AudioSource audioSource;
    ParticleSystem particleSystem;

    private void Update()
    {
        if (particleSystem.isPlaying)
        {
            particleSystem.Stop();
        }
    }

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 0.25f;
        audioSource.clip = pressedSound;
        // ƒ≥стаЇмо ан≥матор з кнопки, на €ку пов≥сили скрипт
        anim = GetComponent<Animator>();
    }

    // якщо тригеритьс€ колайдер кнопки
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "fireball")
        {
            audioSource.Play();
            particleSystem.Emit(25);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // якщо у кнопку прилет≥в не файрбол
        if (collision.tag != "fireball")
        {
            // «м≥нюЇмо в≥дпов≥дний прапорець на true 
            isPressed = true;
            //Debug.Log("Button state = " + isPressed);

            // ѕ≥дключаЇмо тут ан≥мац≥ю
            anim.SetBool("Pressed", isPressed);
        }
    }

    // якщо колайдер кнопки перестаЇ тригеритис€
    private void OnTriggerExit2D(Collider2D collision)
    {
        // якщо у кнопку прилет≥в не файрбол
        if (collision.tag != "fireball")
        {
            // «м≥нюЇмо в≥дпов≥дний прапорець на false
            isPressed = false;
            //Debug.Log("Button state = " + isPressed);

            // ѕ≥дключаЇмо тут ан≥мац≥ю
            anim.SetBool("Pressed", isPressed);
        }
    }
}
