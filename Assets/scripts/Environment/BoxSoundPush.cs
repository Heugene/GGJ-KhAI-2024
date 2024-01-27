using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSoundPush : MonoBehaviour
{
    AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (sound != null)
        {
            sound.Play();
        }
    }
}
