using UnityEngine;
using System.Collections;
using UnityEngine.AI;

/// <summary>
/// Компонент який реалізує поведення клоуна
/// </summary>
public class EnemyClown : MonoBehaviour
{
    public bool isAttaking = false;             // Макер який характеризує чи атакує клоун
    public bool isMoving = false;               // Макер який характеризує чи переміщується клоун

    [SerializeField] float moveSpeed = 2f;      // Швидкість руху
    [SerializeField] float attackCooldown = 2f; // Час перезарядки удару
    [SerializeField] int clownDamage = 1;       // Дамаг за удар
    [SerializeField] float attackRange = 4f;    // Дистанція атаки
    [SerializeField, Header("Показати дистацнію атаки?")]

    private bool drawAtackRange = false;      // Макер для відображення дистанції атаки в інспекторі
    private bool canAttack = true;            // Макер для позначення перезарядки
    private NavMeshAgent navMeshAgent;    // Посилання компонент який відповідає за переміщення
    private Transform player;             // Посилання на гравця


    void Start()
    {
        // Пошук необхідних об'єктів
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Налаштування navMeshAgent
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.speed = moveSpeed;
    }

    private void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, player.position);

        // Якщо гравець в радіусі атаки обєкта 
        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            if (canAttack)
                StartCoroutine(AttackWithCooldown());
        }
        else
        {
            // Йти в сторону гравця
            isMoving = true;
            isAttaking = false;

            if(navMeshAgent.isActiveAndEnabled)
                navMeshAgent.SetDestination(player.position);
        }
    }

    // Нанасення атаки гравцю
    IEnumerator AttackWithCooldown()
    {
        isMoving = false;
        canAttack = false;
        isAttaking = true;

        player.GetComponent<PlayerHealthController>().TakeDamage(clownDamage);
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // Відображення дистації атаки
    private void OnDrawGizmosSelected()
    {
        if (drawAtackRange)
            Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
