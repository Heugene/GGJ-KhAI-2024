using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 7f; // �������� ������� � �������
    [SerializeField]
    private float maxJumpDistance = 5f; // ����������� �������� �������
    [SerializeField]
    private float chargedJumpDistance = 0; // �������� ����� ������� (� ��������� ������)
    [SerializeField]
    private float chargedJumpDistanceMultiplier = 0.4f; // ������ ������

    private Vector2 mousePosition; // ���������� ����
    private Vector2 LastMousePosition; // ������ ���������� ����
    private bool IsMoving = false; // ��������� ����� ����
    private bool IsButtonMovePressed = false; // ��������� ��������� ������ ����
    private bool isPlayerHitEnemy = false; // ����� ��� ���������� �� ������� �������� � �������

    void FixedUpdate()
    {
        // �������� ������� ���� � ������� �����������
        mousePosition = GetMouseWorldPosition();

        if (!IsMoving && IsButtonMovePressed)
        {
            if (maxJumpDistance > chargedJumpDistance)
                chargedJumpDistance += chargedJumpDistanceMultiplier;
            else
                chargedJumpDistance = maxJumpDistance;
        }

        if (IsMoving && !isPlayerHitEnemy)
        {
            // ���������� ��������
            MoveToTarget(LastMousePosition);
        }
    }

    void Update()
    {
        // �������� ������� ���� � ������� �����������
        mousePosition = GetMouseWorldPosition();

        // ������ �������� ���
        // ���� ��������� ������ �����������
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0))
        {
            IsMoving = false;
            IsButtonMovePressed = true;
        }

        // ���� ��������� ������ �����������
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0))
        {
            IsMoving = true;
            IsButtonMovePressed = false;
            isPlayerHitEnemy = false;
            // ��������� ������ ����������� �� ������� ������� �� ������� ����
            Vector2 direction = (mousePosition - (Vector2)transform.position);

            // ������������ ����� ������� �� maxJumpDistance
            LastMousePosition = (Vector2)transform.position + Vector2.ClampMagnitude(direction, chargedJumpDistance);
            chargedJumpDistance = 0;
        }
    }

    /// <summary>
    /// ����� ����������� �� ������ �����
    /// </summary>
    void MoveToTarget(Vector2 LastMousePosition)
    {
        // ��������� ����������� ����� ������� ����������� � ���������� ���������
        transform.position = Vector2.Lerp(transform.position, LastMousePosition, moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// �������, �� ������� ����� ���������� ����
    /// </summary>
    Vector2 GetMouseWorldPosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    /// <summary>
    /// ��������� ������ ����� �� ������������� � �����������
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            isPlayerHitEnemy = true;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            isPlayerHitEnemy = true;
        }
    }
}
