using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxParticleSystem : MonoBehaviour
{
    [SerializeField]
    ParticleSystem particleSystem;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (rb.IsSleeping())
            particleSystem.Stop();
        else
            particleSystem.Play();
    }
}
