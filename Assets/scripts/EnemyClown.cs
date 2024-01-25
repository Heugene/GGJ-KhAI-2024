using UnityEngine;
using System.Collections;
using NavMeshPlus;
using UnityEngine.AI;


public class EnemyClown : MonoBehaviour
{
    public bool isAttaking = false;
    public bool isStanding = false;
    public bool isMoving = false;
    public Transform player;           // Посилання на трансформ гравця

    [SerializeField] float moveSpeed = 2f;      // Швидкість руху клоуна
    [SerializeField] float attackCooldown = 2f; // Час між атаками
    [SerializeField] int clownDamage = 1;       // Значення дамагу
    [SerializeField] float attackRange = 4f;    // Радіус атаки врогів
    [SerializeField, Header("Показати радіус атаки?")]
    private bool drawAtackRange = false;

    private bool canAttack = true;
    private NavMeshAgent navMeshAgent;  // Посилання на NavMeshAgent


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.speed = moveSpeed;

    }

    private void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, player.position);

        // Если игрок в радиусе атаки, начинаем атаку
        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            if (canAttack)
                StartCoroutine(AttackWithCooldown());

            isMoving = false;
            isStanding = true;
        }
        else
        {
            // Иначе двигаемся к целевой позиции
            isMoving = true;
            isStanding = false;
            navMeshAgent.SetDestination(player.position);
        }
    }

    IEnumerator AttackWithCooldown()
    {
        Debug.Log("Attack!");  // TODO: Тут має бути логіка анімки.

        if (canAttack)
        {
            canAttack = false;
            isAttaking = true;
            isStanding = true;
            player.GetComponent<PlayerHealthController>().TakeDamage(clownDamage);
        }

        // Блокуємо атаку до моменту, поки не пройде час перезарядки
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        isAttaking = false;
    }

    //// Відображення радіусу атаки клоуна
    private void OnDrawGizmosSelected()
    {
        if (drawAtackRange)
            Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
