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
    PlayerDash playerDash;
    PlayerJump playerJump;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        playerDash = GetComponent<PlayerDash>();
        playerJump = GetComponent<PlayerJump>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerRotation();

        if (playerJump.IsMoving)
        {
            isJumping = true;
            isStanding = false;
        }
        else if(!playerDash.isDashing)
        {
            isJumping = false;
            isStanding = true;
            isDashing = false;
        }

        if (playerDash.isDashing)
        {
            isJumping = false;
            isStanding = false;
            isDashing = true;
        }

        animator.SetBool("isStanding", isStanding);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isDashing", isDashing);
    }


    public void Freeze()
    {
        //TODO: Do logic to freeze player
    }

    private void PlayerRotation()
    {
        if(playerJump.GetMouseWorldPosition().x < transform.position.x)
            sr.flipX = true;
        else
            sr.flipX = false;
    }
}
