using UnityEngine;
using System.Collections;
using UnityEngine.AI;


public class EnemyClown : MonoBehaviour
{
    public bool isAttaking = false;
    public bool isMoving = false;
    public Transform player;           // Ïîñèëàííÿ íà òðàíñôîðì ãðàâöÿ

    [SerializeField] float moveSpeed = 2f;      // Øâèäê³ñòü ðóõó êëîóíà
    [SerializeField] float attackCooldown = 2f; // ×àñ ì³æ àòàêàìè
    [SerializeField] int clownDamage = 1;       // Çíà÷åííÿ äàìàãó
    [SerializeField] float attackRange = 4f;    // Ðàä³óñ àòàêè âðîã³â
    [SerializeField, Header("Ïîêàçàòè ðàä³óñ àòàêè?")]
    private bool drawAtackRange = false;

    private bool canAttack = true;
    private NavMeshAgent navMeshAgent;  // Ïîñèëàííÿ íà NavMeshAgent


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

        // Åñëè èãðîê â ðàäèóñå àòàêè, íà÷èíàåì àòàêó
        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            if (canAttack)
                StartCoroutine(AttackWithCooldown());
        }
        else
        {
            // Èíà÷å äâèãàåìñÿ ê öåëåâîé ïîçèöèè
            isMoving = true;
            isAttaking = false;

            if(navMeshAgent.isActiveAndEnabled)
                navMeshAgent.SetDestination(player.position);
        }
    }

    IEnumerator AttackWithCooldown()
    {
        Debug.Log("Attack!");  // TODO: Òóò ìàº áóòè ëîã³êà àí³ìêè.

        isMoving = false;
        canAttack = false;
        isAttaking = true;
        player.GetComponent<PlayerHealthController>().TakeDamage(clownDamage);

        // Áëîêóºìî àòàêó äî ìîìåíòó, ïîêè íå ïðîéäå ÷àñ ïåðåçàðÿäêè
        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    //// Â³äîáðàæåííÿ ðàä³óñó àòàêè êëîóíà
    private void OnDrawGizmosSelected()
    {
        if (drawAtackRange)
            Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
