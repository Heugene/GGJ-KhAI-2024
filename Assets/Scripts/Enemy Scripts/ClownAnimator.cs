using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownAnimator : MonoBehaviour
{
    Animator animator;
    EnemyClown clown;
    Transform player;
    SpriteRenderer spriteRenderer;


    void Start()
    {
        animator = GetComponent<Animator>();
        clown = GetComponent<EnemyClown>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x < transform.position.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;

        if(clown.isMoving)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isMoving", true);
        }

        if (clown.isAttaking)
        {
            animator.SetBool("isAttacking", true);
            animator.SetBool("isMoving", false);
        }
    }
}
