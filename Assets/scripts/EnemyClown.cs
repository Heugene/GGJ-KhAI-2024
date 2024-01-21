using UnityEngine;
using System.Collections;

public class EnemyClown : MonoBehaviour
{
    [SerializeField] float speed = 3f; // Скорость движения врага
    [SerializeField] float attackRange = 2f; // Радиус атаки врага
    [SerializeField] float attackCooldown = 2f; // Время между атаками

    private Transform player; // Ссылка на трансформ игрока
    private bool canAttack = true; // Флаг разрешения атаки

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Проверка, находится ли игрок в радиусе атаки
        if (distanceToPlayer <= attackRange)
        {
            // Если атака не на перезарядке атакуем игрока
            if (canAttack) 
                StartCoroutine(AttackWithCooldown());
        }
        else
        {
            // Если находится в радиусе следовать за игроку
            ChasePlayer();
        }
    }

    // Следовать за игроком
    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        transform.Translate(direction * speed * Time.deltaTime);
    }

    IEnumerator AttackWithCooldown()
    {
        Debug.Log("Attack!");  // TODO: Тут должна быть логика анимки и нанесения дамага.

        // Блокируем атаку до момента пока не пройдёт время перезарядки
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
