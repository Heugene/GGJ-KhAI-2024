using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChallengeLogic : MonoBehaviour
{
    internal bool Solved { get; private set; }
    private GameObject player; // ����
    
    // ��'���� ������� ��� ��� � ��������, ��� ����� ���� � ��� ���������� ���� ����������� ����
    [SerializeField] private ButtonGroupLogic Area1; 
    [SerializeField] private ButtonGroupLogic Area2;
    [SerializeField] private ButtonGroupLogic Area3;
    [SerializeField] private ButtonGroupLogic Area4;
    [SerializeField] private Transform Lever;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Pentagram").GetComponent<PentagramLogic>().End += EndGame;
        player = GameObject.FindGameObjectWithTag("Player");

        Area1.LevelCompleted += Area1_CompleteActions;
        Area2.LevelCompleted += Area2_CompleteActions;
        Area3.LevelCompleted += Area3_CompleteActions;
        Area4.LevelCompleted += Area4_CompleteActions;

        Area2.gameObject.SetActive(false);
        Area3.gameObject.SetActive(false);
        Area4.gameObject.SetActive(false);
    }


    // ������ ����������� ������, ��������, ������������� �����, ������ ����� ������


    // ĳ�, ���� ������� ����� ���� � ��������
    private void Area1_CompleteActions()
    {
        Debug.Log("Area1 COMPLETED");

        // ���������� ���� 2
        Area2.gameObject.SetActive(true);

        //����
        //GameObject.FindGameObjectWithTag("Pentagram").GetComponent<PentagramLogic>().Activation();

    }

    // ĳ�, ���� ������� ����� ���� � ��������
    private void Area2_CompleteActions()
    {
        Debug.Log("Area2 COMPLETED");

        // ���������� ���� 3
        Area3.gameObject.SetActive(true);
    }

    // ĳ�, ���� ������� ����� ���� � ��������
    private void Area3_CompleteActions()
    {
        Debug.Log("Area3 COMPLETED");

        // ���������� ���� 4
        Area4.gameObject.SetActive(true);
    }

    // ĳ�, ���� ������� �������� ���� � ��������
    private void Area4_CompleteActions()
    {
        Debug.Log("Area4 COMPLETED");
        // �� �������� ������.��� ��������������.������, �������� ��������� ���������� ������ʲ����������
        
        Lever.gameObject.SetActive(true);

        GameObject.FindGameObjectWithTag("Pentagram").GetComponent<PentagramLogic>().Activation();

        // ������� ������� ��� ��������� ������, ��� ��������� ���������� ����������.
        player.GetComponentInChildren<TrailRenderer>().time = 45;

    }
    private void EndGame()
    {
        //ĳ� ���� ����������� ����������

        // ��������� ����� ��� ��������� ������
        player.GetComponentInChildren<TrailRenderer>().time = 5;
    }
}
