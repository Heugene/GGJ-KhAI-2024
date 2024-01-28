using UnityEngine;

/// <summary>
/// ���������, ���� ������ ���� ������ � Animator ����� ��'����
/// </summary>
public class ClownAnimator : MonoBehaviour
{
    private Animator animator; // ��������� �� ������� ������� �����
    private EnemyClown clown;  // ��������� �� ��������� EnemyClown ������� �����
    private Transform player;  // ��������� �� ������
    [SerializeField] private Transform fireballSpawnPoint;

    

    void Start()
    {
        // ����� ���������� ��'����
        fireballSpawnPoint = GameObject.FindWithTag("fireball Spawn Point").transform;
        animator = GetComponent<Animator>();
        clown = GetComponent<EnemyClown>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        SettingDirection();

        // ���� ����� �����������
        if (clown.isMoving)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isMoving", true);
        }

        // ���� ����� �����
        if (clown.isAttaking)
        {
            animator.SetBool("isAttacking", true);
            animator.SetBool("isMoving", false);
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

    // ������������ �������� �������
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
}
