using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleSystem : MonoBehaviour
{
    [SerializeField]
    ParticleSystem particleSystem;
    [SerializeField]
    PlayerDash playerDash;

    void FixedUpdate()
    {
        if (playerDash.isDashing)
            particleSystem.Play();
        else
            particleSystem.Stop();
    }
}
