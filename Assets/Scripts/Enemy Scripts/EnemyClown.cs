using UnityEngine;
using System.Collections;
using UnityEngine.AI;

/// <summary>
/// Компонент який реалізує поведення клоуна
/// </summary>
public class EnemyClown : MonoBehaviour
{
    spawnFireball _spawnFireball;

    public bool isAttaking = false;             // Маркер який характеризує чи атакує клоун
    public bool isMoving = false;               // Маркер який характеризує чи переміщується клоун
    public bool isDead = false;                 // Маркер який характеризує чи вмер клоун
    public bool isStoped = false;               // Маркер який характеризує чи потрібно зупинити логіку скрипта
    public int clownDamage = 1;                 // Дамаг за удар

    [SerializeField] float moveSpeed = 2f;      // Швидкість руху
    [SerializeField] float attackCooldown = 2f; // Час перезарядки удару
    [SerializeField] float attackRange = 4f;    // Дистанція атаки

    [SerializeField, Header("Показати дистацнію атаки?")]
    private bool drawAtackRange = false;      // Макер для відображення дистанції атаки в інспекторі
    private bool canAttack = true;            // Макер для позначення перезарядки
    private NavMeshAgent navMeshAgent;    // Посилання компонент який відповідає за переміщення
    private Transform player;             // Посилання на гравця


    void Start()
    {
        _spawnFireball = GameObject.FindWithTag("fireball Spawn Point").GetComponent<spawnFireball>();

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
        if (isStoped) return;

        float distanceToTarget = Vector2.Distance(transform.position, player.position);
        // Якщо гравець в радіусі атаки обєкта 
        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            isAttaking = true;
            isMoving = false;
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

    public IEnumerator AttackWithCooldown()
    {
        isMoving = false;
        canAttack = false;
        StartCoroutine(_spawnFireball.SpawnFireballs());
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
