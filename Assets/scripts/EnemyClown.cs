using UnityEngine;
using System.Collections;

public class EnemyClown : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f; // Швидкість руху клоуна
    [SerializeField] float attackRange = 4f; // Радіус атаки врогів
    [SerializeField] float attackCooldown = 2f; // Час між атаками
    [SerializeField] int clownDamage = 1; // Значення дамагу

    [SerializeField, Header("Показати радіус атаки?")]
    private bool drawAtackRange = false;

    public Transform player; // Посилання на трансформ гравця
    private bool canAttack = true; // Флаг для дозвілу атаки
    public bool isAttaking = false;
    public bool isStanding = false;
    public bool isMoving = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Перевірка, чи гравець перебуває в радіусі атаки
        if (distanceToPlayer <= attackRange)
        {
            // Якщо атака не на перезарядці атакуємо гравця
            if (canAttack) 
                StartCoroutine(AttackWithCooldown());
            isMoving = false;
            isStanding = true;
        }
        else
        {
            // Якщо знаходиться в радіусі слідувати за гравцем
            ChasePlayer();
        }
    }

    // Слідувати за гравцем
    void ChasePlayer()
    {
        isMoving = true;
        isStanding = false;
        isAttaking = false;
        Vector2 direction = (player.position - transform.position).normalized;

        transform.Translate(direction * moveSpeed * Time.deltaTime);
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

    // Відображення радіусу атаки клоуна
    private void OnDrawGizmosSelected()
    {
        if (drawAtackRange)
            Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
