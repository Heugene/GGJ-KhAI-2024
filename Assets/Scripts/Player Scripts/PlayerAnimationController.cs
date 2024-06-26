using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    SpriteRenderer sr;
    bool isStanding;
    bool isJumping;
    bool isDashing;
    PlayerDash playerDash;
    PlayerJump playerJump;
    PlayerHealthController playerHealthController;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        playerDash = GetComponent<PlayerDash>();
        playerJump = GetComponent<PlayerJump>();
        playerHealthController = GetComponent<PlayerHealthController>();
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


    public void Freeze(bool isFreeze)
    {
        playerDash.isFreezed = isFreeze;
        playerJump.isFreezed = isFreeze;
        playerHealthController.isImmuneToDamage = isFreeze;
    }

    private void PlayerRotation()
    {
        if(playerJump.GetMouseWorldPosition().x < transform.position.x)
            sr.flipX = true;
        else
            sr.flipX = false;
    }
}
