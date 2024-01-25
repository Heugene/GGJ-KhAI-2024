using UnityEngine;
using System.Collections;
using NavMeshPlus;
using UnityEngine.AI;


public class EnemyClown : MonoBehaviour
{
    public bool isAttaking = false;
    public bool isStanding = false;
    public bool isMoving = false;
    public Transform player;           // ��������� �� ��������� ������

    [SerializeField] float moveSpeed = 2f;      // �������� ���� ������
    [SerializeField] float attackCooldown = 2f; // ��� �� �������
    [SerializeField] int clownDamage = 1;       // �������� ������
    [SerializeField] float attackRange = 4f;    // ����� ����� �����
    [SerializeField, Header("�������� ����� �����?")]
    private bool drawAtackRange = false;

    private bool canAttack = true;
    private NavMeshAgent navMeshAgent;  // ��������� �� NavMeshAgent


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

        // ���� ����� � ������� �����, �������� �����
        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            if (canAttack)
                StartCoroutine(AttackWithCooldown());

            isMoving = false;
            isStanding = true;
        }
        else
        {
            // ����� ��������� � ������� �������
            isMoving = true;
            isStanding = false;
            navMeshAgent.SetDestination(player.position);
        }
    }

    IEnumerator AttackWithCooldown()
    {
        Debug.Log("Attack!");  // TODO: ��� �� ���� ����� �����.

        if (canAttack)
        {
            canAttack = false;
            isAttaking = true;
            isStanding = true;
            player.GetComponent<PlayerHealthController>().TakeDamage(clownDamage);
        }

        // ������� ����� �� �������, ���� �� ������ ��� �����������
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        isAttaking = false;
    }

    //// ³���������� ������ ����� ������
    private void OnDrawGizmosSelected()
    {
        if (drawAtackRange)
            Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
