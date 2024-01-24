using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 7f; // �������� ������� � �������
    [SerializeField]
    private float maxJumpDistance = 5f; // ����������� �������� �������
    private float chargedJumpDistance = 0; // �������� ����� ������� (� ��������� ������)
    [SerializeField]
    private float chargedJumpDistanceMultiplier = 0.4f; // ������ ������
    [SerializeField]
    private float DashSpeed = 12;
    private float DashSpeedTemp = 0;
    [SerializeField]
    private float DashCooldown = 1;
    private float DashCooldownTemp = 0;
    [SerializeField]
    private float DashSpeedReducer = 0.05f;
    [SerializeField]
    private ItemType item = ItemType.None;

    private Vector2 mousePosition; // ���������� ����
    private Vector2 LastMousePosition; // ������ ���������� ����
    public bool IsMoving = false; // ��������� ����� ����
    public bool IsButtonJumpPressed = false; // ��������� ��������� ������ ����
    public bool isPlayerHitEnemy = false; // ����� ��� ���������� �� ������� �������� � �������
    [SerializeField]
    public bool isCanDash = false; // ����� �� ����� ������� ���
    [SerializeField]
    public bool isDashing = false; // ������ �� ����� ���


    private void Start()
    {
        DashSpeedTemp = DashSpeed;
        LastMousePosition = transform.position;
    }

    // ������
    void FixedUpdate()
    {
        // �������� ������� ���� � ������� �����������
        mousePosition = GetMouseWorldPosition();

        if (item == ItemType.Ketchup)
        {
            if(isCanDash)
                MakeDash();

            if (isDashing)
            {
                CalculateDash();
                CalculateDashReload();
            }
        }

        CalculateJumpDistance();

        if (IsMoving && !isPlayerHitEnemy && isCanDash == false)
            MoveToTarget(LastMousePosition);// ���������� ��������}
    }

    // input �������
    void Update()
    {
        // �������� ������� ���� � ������� �����������
        mousePosition = GetMouseWorldPosition();

        // ������ �������� ���
        // ���� ��������� ������ �����������
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0))
        {
            IsMoving = false;
            IsButtonJumpPressed = true;
        }

        // ���� ��������� ������ �����������
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0) && isDashing == false)
        {
            isCanDash = false;
            IsMoving = true;
            IsButtonJumpPressed = false;
            isPlayerHitEnemy = false;
            // ��������� ������ ����������� �� ������� ������� �� ������� ����
            Vector2 direction = (mousePosition - (Vector2)transform.position);

            // ������������ ����� ������� �� maxJumpDistance
            LastMousePosition = (Vector2)transform.position + Vector2.ClampMagnitude(direction, chargedJumpDistance);
            chargedJumpDistance = 0;
        }

        var distance = ((Vector2)transform.position - LastMousePosition);
        if(distance.magnitude < 0.1)
        {
            IsMoving = false;
        }


        if (Input.GetKeyUp(KeyCode.Mouse1) && item == ItemType.Ketchup && !isDashing)
        {
            isCanDash = true;
            isDashing = true;
        }
    }

    // ������� ������
    void CalculateJumpDistance()
    {
        if (!IsMoving && IsButtonJumpPressed)
        {
            if (maxJumpDistance > chargedJumpDistance)
                chargedJumpDistance += chargedJumpDistanceMultiplier;
            else
                chargedJumpDistance = maxJumpDistance;
        }
    }

    // ���������� �������� ���� �� ������� �������� DashSpeedReducer
    void CalculateDash()
    {
        if (isCanDash && isDashing)
        {
            DashSpeed -= DashSpeedReducer;
            if(DashSpeed <= 0)
            {
                isCanDash = false;
                DashSpeed = DashSpeedTemp;
            }
        }
    }

    // ����������� ���� � ��������
    void CalculateDashReload()
    {
        if (!isCanDash)
        {
            DashCooldownTemp += Time.fixedDeltaTime;
            if (DashCooldownTemp >= DashCooldown)
            {
                DashCooldownTemp = 0;
                isDashing = false;
            }
        }
    }

    // ����� ����������� �� ������ �����
    void MoveToTarget(Vector2 LastMousePosition)
    {
        // ��������� ����������� ����� ������� ����������� � ���������� ���������
        transform.position = Vector2.MoveTowards(transform.position, LastMousePosition, moveSpeed * Time.deltaTime);
    }

    // �������� ����
    void MakeDash()
    {
        // ��������� ������ ����������� �� ������� ������� �� ������� ����
        Vector2 direction = mousePosition - (Vector2)transform.position;

        // ��������� MoveTowards ��� �������� � �������
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + direction.normalized, DashSpeed * Time.deltaTime);

        // ��������� LastMousePosition
        LastMousePosition = transform.position;
    }

    // �������, �� ������� ����� ���������� ����
    public Vector2 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // ��������� ������ ����� �� ������������� � �����������
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