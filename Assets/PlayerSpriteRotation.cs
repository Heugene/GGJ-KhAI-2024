using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteRotation : MonoBehaviour
{
    Movement movement;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        movement = GetComponent<Movement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(movement.GetMouseWorldPosition().x < transform.position.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }
}
