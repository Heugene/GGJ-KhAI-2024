using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    Rigidbody2D rb;
    SpriteRenderer sr;
    bool isStanding;
    bool isJumping;
    bool isDashing;
    Movement movement;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerRotation();

        if (movement.IsMoving)
        {
            isJumping = true;
            isStanding = false;
        }
        else if(!movement.isDashing)
        {
            isJumping = false;
            isStanding = true;
            isDashing = false;
        }

        if (movement.isDashing)
        {
            isJumping = false;
            isStanding = false;
            isDashing = true;
        }

        animator.SetBool("isStanding", isStanding);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isDashing", isDashing);
    }

    private void PlayerRotation()
    {
        if(movement.GetMouseWorldPosition().x < transform.position.x)
            sr.flipX = true;
        else
            sr.flipX = false;
    }
}
