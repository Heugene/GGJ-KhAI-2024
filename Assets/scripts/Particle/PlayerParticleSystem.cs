using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleSystem : MonoBehaviour
{
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] PlayerDash playerDash;
    [SerializeField] Material ketchupMaterial;
    [SerializeField] Material mayonnaiseMaterial;


    private void Awake()
    {
        playerDash.onActivateTrail += handlePatricleMaterial;
            if (ketchupMaterial == null) Debug.LogError("");
    }

    void FixedUpdate()
    {
        if (playerDash.isDashing)
            particleSystem.Play();
        else
            particleSystem.Stop();
    }

    private void handlePatricleMaterial(ItemType type)
    {
        var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();

        switch (type)
        {
            case ItemType.Mayonnaise:
                renderer.material = mayonnaiseMaterial;
                break;
            case ItemType.Ketchup:
                renderer.material = ketchupMaterial;
                break;

            default:
                break;
        }
    }
}
