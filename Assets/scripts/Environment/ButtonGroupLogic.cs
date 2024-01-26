using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGroupLogic : MonoBehaviour
{
    internal event Action LevelCompleted; // ���� ����������� ������ �����

    // �������������� ����������, ��� �������� ���������, �� �������� ���� ���� � �������� (������� ��� ������������ �����������)
    internal bool Passed { get; private set; }

    // ����������, ��� ������� True, ����� ���� �� ������ ���� ����� � ������ ���� ��������
    private bool Activated 
    {
        get => Array.TrueForAll(GetComponentsInChildren<ButtonPressLogic>(), x => x.isPressed.Equals(true));
    }

    private void FixedUpdate()
    {
        // ���� � ������ ���� �� ������ �������� ���������, � ����� �� �� ����������, ������� ���� �� ���������.
        if (Activated && Passed == false) 
        {
            Passed = true;
            LevelCompleted?.Invoke();
        }
    }
}
