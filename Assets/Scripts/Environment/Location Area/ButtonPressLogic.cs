using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressLogic : MonoBehaviour
{
    internal bool isPressed { get; private set; } = false; // �� ��������� ������
    private Animator anim;

    private void Start()
    {
        // ĳ����� ������� � ������, �� ��� ������� ������
        anim = GetComponent<Animator>();
    }

    // ���� ����������� �������� ������
    private void OnTriggerStay2D(Collider2D collision)
    {
        // ���� � ������ ������� �� �������
        if (collision.tag != "fireball")
        {
            // ������� ��������� ��������� �� true 
            isPressed = true;
            //Debug.Log("Button state = " + isPressed);

            // ϳ�������� ��� �������
            anim.SetBool("Pressed", isPressed);
        }
    }

    // ���� �������� ������ ������� �����������
    private void OnTriggerExit2D(Collider2D collision)
    {
        // ���� � ������ ������� �� �������
        if (collision.tag != "fireball")
        {
            // ������� ��������� ��������� �� false
            isPressed = false;
            //Debug.Log("Button state = " + isPressed);

            // ϳ�������� ��� �������
            anim.SetBool("Pressed", isPressed);
        }
    }
}
