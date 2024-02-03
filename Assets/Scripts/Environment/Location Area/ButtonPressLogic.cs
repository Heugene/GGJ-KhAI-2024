using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressLogic : MonoBehaviour
{
    internal bool isPressed { get; private set; } = false; // �� ��������� ������
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
        // ĳ����� ������� � ������, �� ��� ������� ������
        anim = GetComponent<Animator>();
    }

    // ���� ����������� �������� ������
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
        // ���� � ������ ������� �� �������
        if (collision.tag != "fireball")
        {
            // ������� ��������� ��������� �� true 
            isPressed = true;
            //Debug.Log("Button state = " + isPressed);

            // ϳ�������� ��� �������
            anim.SetBool("Pressed", isPressed);
        }
    }

    // ���� �������� ������ ������� �����������
    private void OnTriggerExit2D(Collider2D collision)
    {
        // ���� � ������ ������� �� �������
        if (collision.tag != "fireball")
        {
            // ������� ��������� ��������� �� false
            isPressed = false;
            //Debug.Log("Button state = " + isPressed);

            // ϳ�������� ��� �������
            anim.SetBool("Pressed", isPressed);
        }
    }
}
