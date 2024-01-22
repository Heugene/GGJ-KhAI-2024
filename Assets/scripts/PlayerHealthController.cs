using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public int health = 5; // ������� ��
    private int numOfParts; // ʳ������ �������� �������, ��� ���������� ������� ��;

    public Image[] parts; // ������ �������� �������

    public Sprite fullBodyPart; // ������ ��������� ������� ��������
    public Sprite emptyBodyPart; // ������ �������� ������� ��������
    public Sprite fullTailPart; // ������ ��������� �������� ��������
    public Sprite emptyTailPart; // ������ �������� �������� ��������
    public Sprite fullHeadPart; // ������ ��������� ������� ��������
    public Sprite emptyHeadPart; // ������ �������� ������� ��������


    // ����� ��������� ������
    internal void TakeDamage(int damage)
    {
        if (health > 0)
        {
            health -= damage; // ³������� ����� �� ��

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
        // �������� ���� 䳿 ���� �����
    }


    // Start is called before the first frame update
    void Start()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        numOfParts = health;
        parts = player.GetComponentsInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}