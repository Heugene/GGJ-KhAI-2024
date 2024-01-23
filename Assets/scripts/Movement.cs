using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 7f; // �������� ������� � �������
    [SerializeField]
    private float dashSpeed = 7f; // �������� ������� � �������
    private float dashSpeedTemp = 0;
    [SerializeField]
    private float dashReloadTime = 1f;
    [SerializeField]
    private float maxDashTime = 5f; 
    [SerializeField]
    private float maxJumpDistance = 5f; // ����������� �������� �������
    [SerializeField]
    private float chargedJumpDistance = 0; // �������� ����� ������� (� ��������� ������)
    [SerializeField]
    private float chargedJumpDistanceMultiplier = 0.4f; // ������ ������
    [SerializeField]
    private ItemType itemEquipped;

    private float dashTickTimeToReload = 0;
    private float forDashReaload = 0;

    private Vector2 mousePosition; // ���������� ����
    private Vector2 LastMousePosition; // ������ ���������� ����
    private bool isMoving = false; // ��������� ����� ����
    private bool IsButtonJumpPressed = false; // ��������� ��������� ������ ����
    private bool isPlayerHitEnemy = false; // ����� ��� ���������� �� ������� �������� � �������
    private bool isCanDashing = false;
    private bool isRKMPressed = false;

    private void Start()
    {
        dashSpeedTemp = dashSpeed;
    }

    void FixedUpdate()
    {
        // �������� ������� ���� � ������� �����������
        mousePosition = GetMouseWorldPosition();

        if(itemEquipped == ItemType.Ketchup)
        {
            calculateTimeOfDash();
            ReloadDash();

            if (isCanDashing)
            {
                // ����� ��������
                makeDash();
            }
        }

        calculateJumpDistanceCharge();

        if (isMoving && !isPlayerHitEnemy && isCanDashing == false)
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
            isMoving = false;
            IsButtonJumpPressed = true;
        }

        // ���� ��������� ������ �����������
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0))
        {
            isMoving = true;
            IsButtonJumpPressed = false;
            isPlayerHitEnemy = false;
            // ��������� ������ ����������� �� ������� ������� �� ������� ����
            Vector2 direction = (mousePosition - (Vector2)transform.position);

            // ������������ ����� ������� �� maxJumpDistance
            LastMousePosition = (Vector2)transform.position + Vector2.ClampMagnitude(direction, chargedJumpDistance);
            chargedJumpDistance = 0;
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) && itemEquipped == ItemType.Ketchup)
        {
            isCanDashing = true;
            isRKMPressed = true;
        }
    }




    void calculateTimeOfDash()
    {
        if (isCanDashing == true && isRKMPressed)
        {
            dashTickTimeToReload += Time.fixedDeltaTime;
            dashSpeed -= 0.05f;
            if (dashTickTimeToReload >= maxDashTime || dashSpeed <= 0)
            {
                dashTickTimeToReload = 0f;
                isCanDashing = false;
                isRKMPressed = false;
                LastMousePosition = transform.position;
                dashSpeed = dashSpeedTemp;
            }
        }
    }

    void ReloadDash()
    {
        if (isCanDashing == false && isRKMPressed)
        {
            forDashReaload += Time.fixedDeltaTime;
            if (forDashReaload >= dashReloadTime)
            {
                forDashReaload = 0f;
                isCanDashing = true;
            }
        }
    }

    void calculateJumpDistanceCharge()
    {
        if (!isMoving && IsButtonJumpPressed)
        {
            if (maxJumpDistance > chargedJumpDistance)
                chargedJumpDistance += chargedJumpDistanceMultiplier;
            else
                chargedJumpDistance = maxJumpDistance;
        }
    }

    /// ����� ����������� �� ������ �����
    void MoveToTarget(Vector2 LastMousePosition)
    {
        // ��������� ����������� ����� ������� ����������� � ���������� ���������
        transform.position = Vector2.Lerp(transform.position, LastMousePosition, moveSpeed * Time.deltaTime);
    }

    /// �������, �� ������� ����� ���������� ����
    Vector2 GetMouseWorldPosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    /// ��������� ������ ����� �� ������������� � �����������
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

    private void makeDash()
    {
        // ��������� ����������� � ������� ������� � ���������� ���������
        transform.position = Vector2.MoveTowards(transform.position, mousePosition, Time.deltaTime * dashSpeed);
    }
}
