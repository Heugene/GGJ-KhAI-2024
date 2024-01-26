using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressLogic : MonoBehaviour
{
    private bool isPressed = false; // �� ��������� ������
    private Animator anim;

    private void Start()
    {
        // ĳ����� ������� � ������, �� ��� ������� ������
        anim = GetComponent<Animator>();
    }

    // ���� ����������� �������� ������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ������� ��������� ��������� �� true 
        isPressed = true;
        Debug.Log("Button state = " + isPressed);

        // ϳ�������� ��� �������
        anim.SetBool("Pressed", isPressed);
    }

    // ���� �������� ������ ������� �����������
    private void OnTriggerExit2D(Collider2D collision)
    {
        // ������� ��������� ��������� �� false
        isPressed = false;
        Debug.Log("Button state = " + isPressed);

        // ϳ�������� ��� �������
        anim.SetBool("Pressed", isPressed);
    }
}
