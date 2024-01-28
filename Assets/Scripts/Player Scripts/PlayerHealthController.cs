using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public int health { get; private set; } = 5; // ������� ��
    public bool isImmuneToDamage = false;
    private int numOfParts; // ʳ������ �������� �������, ��� ���������� ������� ��;
    private Image[] parts; // ������ �������� �������
    private Animator anim; // ������� �����

    [SerializeField] private Sprite fullBodyPart; // ������ ��������� ������� ��������
    [SerializeField] private Sprite emptyBodyPart; // ������ �������� ������� ��������
    [SerializeField] private Sprite fullTailPart; // ������ ��������� �������� ��������
    [SerializeField] private Sprite emptyTailPart; // ������ �������� �������� ��������
    [SerializeField] private Sprite fullHeadPart; // ������ ��������� ������� ��������
    [SerializeField] private Sprite emptyHeadPart; // ������ �������� ������� ��������


    // ����� ��������� ������
    internal void TakeDamage(int damage)
    {
        if (isImmuneToDamage) return;

        if (health > 0)
        {
            health -= damage; // ³������ ����� �� ��

            if (health <= 0) // ���� 0 �� ����� ��, �������� � �������� � ������
            {
                health = 0;
                RefreshHealthDisplay();
                Die();
                return;
            }
            RefreshHealthDisplay();

        }
    }

    // ����� ���������� ��
    internal void Heal(int healAmount)
    {
        if (health > 0)
        {
            health += healAmount; // ³��������� �� �� ������ �����

            // ��������, ��� �� ����� ���� �������� ����� ��������� ��.
            if (health > numOfParts)
            {
                health = numOfParts;
            }

            RefreshHealthDisplay();
        }
    }
    
    // ����� ��������� ����������� ��
    internal void RefreshHealthDisplay()
    {
        for (int i = 0; i < health; i++)
        {
            if (i == 0)
            {
                parts[i].sprite = fullTailPart;
            }
            else if (i == numOfParts - 1)
            {
                parts[i].sprite = fullHeadPart;
            }
            else 
            {
                parts[i].sprite = fullBodyPart;
            }
        }
        for (int i = health; i < numOfParts; i++)
        {
            if (i == 0)
            {
                parts[i].sprite = emptyTailPart;
            }
            else if (i == numOfParts - 1)
            {
                parts[i].sprite = emptyHeadPart;
            }
            else
            {
                parts[i].sprite = emptyBodyPart;
            }
        }
    }

    // �������
    internal void Die()
    {
        // �������� ������� �����
        anim.SetBool("isDead", true);

        // ��������� ����� ������
        GetComponent<PlayerAnimationController>().Freeze(true);

       StartCoroutine( BackToMenu() );
    }

    // ������� � ���� � ���������
    private IEnumerator BackToMenu()
    {
        // ĳ����� �������� �������
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        
        // ³��������� ������� � ����
        SceneManager.LoadScene(0);
    }


    // Start is called before the first frame update
    void Start()
    {
        GameObject healthBar = GameObject.FindGameObjectWithTag("HealthBar");
        numOfParts = health;
        parts = healthBar.GetComponentsInChildren<Image>();
        anim = GetComponent<Animator>();
    }
}
