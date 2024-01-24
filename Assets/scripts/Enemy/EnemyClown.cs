using UnityEngine;
using System.Collections;

public class EnemyClown : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f; // �������� ���� ������
    [SerializeField] float attackRange = 4f; // ����� ����� �����
    [SerializeField] float attackCooldown = 2f; // ��� �� �������
    [SerializeField] int clownDamage = 1; // �������� ������

    [SerializeField, Header("�������� ����� �����?")]
    private bool drawAtackRange = false;

    private Transform player; // ��������� �� ��������� ������
    private bool canAttack = true; // ���� ��� ������ �����

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // ��������, �� ������� �������� � ����� �����
        if (distanceToPlayer <= attackRange)
        {
            // ���� ����� �� �� ����������� ������� ������
            if (canAttack) 
                StartCoroutine(AttackWithCooldown());
        }
        else
        {
            // ���� ����������� � ����� �������� �� �������
            ChasePlayer();
        }
    }

    // �������� �� �������
    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    IEnumerator AttackWithCooldown()
    {
        Debug.Log("Attack!");  // TODO: ��� �� ���� ����� �����.

        if (canAttack)
        {
            canAttack = false;
            player.GetComponent<PlayerHealthController>().TakeDamage(clownDamage);
        }

        // ������� ����� �� �������, ���� �� ������ ��� �����������
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // ³���������� ������ ����� ������
    private void OnDrawGizmosSelected()
    {
        if (drawAtackRange)
            Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
