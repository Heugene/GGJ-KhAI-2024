using UnityEngine;
using System.Collections;

public class EnemyClown : MonoBehaviour
{
    [SerializeField] float speed = 2f; // �������� �������� �����
    [SerializeField] float attackRange = 4f; // ������ ����� �����
    [SerializeField] float attackCooldown = 2f; // ����� ����� �������

    private Transform player; // ������ �� ��������� ������
    private bool canAttack = true; // ���� ���������� �����

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // ��������, ��������� �� ����� � ������� �����
        if (distanceToPlayer <= attackRange)
        {
            // ���� ����� �� �� ����������� ������� ������
            if (canAttack) 
                StartCoroutine(AttackWithCooldown());
        }
        else
        {
            // ���� ��������� � ������� ��������� �� ������
            ChasePlayer();
        }
    }

    // ��������� �� �������
    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        transform.Translate(direction * speed * Time.deltaTime);
    }

    IEnumerator AttackWithCooldown()
    {
        Debug.Log("Attack!");  // TODO: ��� ������ ���� ������ ������ � ��������� ������.
        int testClownDMG = 1; // ������� �������� ������


        if (canAttack)
        {
            player.GetComponent<PlayerHealthController>().TakeDamage(testClownDMG);
            canAttack = false;
        }

        // ��������� ����� �� ������� ���� �� ������ ����� �����������
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
