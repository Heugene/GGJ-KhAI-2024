using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Компонент, який передає стан клоуна в Animator цього об'єкту
/// </summary>
public class ClownAnimator : MonoBehaviour
{
    [SerializeField] private Transform fireballSpawnPoint;

    private Animator animator;     // Посилання на аніматок данного обєкту
    private EnemyClown clown;      // Посилання на компонент EnemyClown данного обєкту
    private NavMeshAgent navMesh;  // Посилання на компонент NavMeshAgent данного обєкту
    private Transform player;      // Посилання на гравця
    private bool isFreeze = false;


    void Start()
    {
        // Пошук необхідних об'єктів
        fireballSpawnPoint = GameObject.FindWithTag("fireball Spawn Point").transform;
        animator = GetComponent<Animator>();
        clown = GetComponent<EnemyClown>();
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isFreeze == false) 
        {
            SettingDirection();

            // Якщо клоун переміщується
            if (clown.isMoving)
            {
                animator.SetBool("isAttacking", false);
                animator.SetBool("isMoving", true);
            }

            // Якщо клоун атакує
            if (clown.isAttaking)
            {
                animator.SetBool("isAttacking", true);
                animator.SetBool("isMoving", false);
            }
        }

        if (clown.isDead)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isMoving", false);
            animator.SetBool("isFall", false);
            animator.SetBool("isCanStandUp", false);
            animator.SetBool("isDead", true);
        }
    }

    // Встановлення напрямку погляду
    private void SettingDirection()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (player.transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
            fireballSpawnPoint.localPosition = new Vector2(-1.3f, -0.9f);
        }
        else
        {
            spriteRenderer.flipX = false;
            fireballSpawnPoint.localPosition = new Vector2(1.3f, -0.9f);
        }    
    }

    // Зупиняє та повертає роботу логіки клоуна
    public void Freeze(bool isFreezed)
    {
        isFreeze = isFreezed;
        clown.isStoped  = isFreezed;
        navMesh.enabled = !isFreezed;
    }
}
