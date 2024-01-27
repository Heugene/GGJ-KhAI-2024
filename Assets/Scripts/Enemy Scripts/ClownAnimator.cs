using UnityEngine;

/// <summary>
/// ���������, ���� ������ ���� ������ � Animator ����� ��'����
/// </summary>
public class ClownAnimator : MonoBehaviour
{
    private Animator animator; // ��������� �� ������� ������� �����
    private EnemyClown clown;  // ��������� �� ��������� EnemyClown ������� �����
    private Transform player;  // ��������� �� ������


    void Start()
    {
        // ����� ���������� ��'����
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
    }

    // ������������ �������� �������
    private void SettingDirection()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (player.transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }    
    }
}
