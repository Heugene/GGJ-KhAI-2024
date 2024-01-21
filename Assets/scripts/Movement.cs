using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Movement : MonoBehaviour
{
    // ���� ������
    public float jumpForceMultiplier = 5f;
    // ������������ ���� ������
    public float maxJumpForce = 20f;
    // ������������ ��������� �� ������� ����� �������� �����
    public float stoppingDistance = 2f;

    // ������� �� �� �������� ������
    private bool isChargingJump = false;
    private float jumpForce;

    void Update()
    {
        // ��������� ������� ������ ��� ����������� ������� ������
        if (Input.GetKey(KeyCode.Space))
        {
            isChargingJump = true;
            jumpForce += Time.deltaTime * jumpForceMultiplier;
            jumpForce = Mathf.Clamp(jumpForce, 0f, maxJumpForce);
        }

        // ��������� ������� ������ - ��������� ������
        if (Input.GetKeyUp(KeyCode.Space) && isChargingJump)
        {
            Jump();
        }
    }

    void Jump()
    {
        // �������� ����������� ����
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 jumpDirection = (targetPosition - transform.position).normalized;

        // ��������� ���� ����� � ����������� ����
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);

        // ������ ��������� � ��������������� ������� ��� ���������� � ���������
        float decelerationForce = -rb.velocity.normalized.magnitude * rb.mass / stoppingDistance;
        rb.AddForce(decelerationForce * rb.velocity.normalized, ForceMode2D.Impulse);

        // ���������� ���������� �������
        isChargingJump = false;
        jumpForce = 0f;
    }
}
