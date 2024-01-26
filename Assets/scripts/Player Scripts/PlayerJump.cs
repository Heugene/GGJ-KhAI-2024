using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    PlayerDash playerDash;

    [SerializeField] private float moveSpeed = 7f; // �������� ������� � �������
    [SerializeField] private float maxJumpDistance = 5f; // ����������� �������� �������
    [SerializeField] private float maxJumpReload = 0.5f;
    [SerializeField] private float chargedJumpDistanceMultiplier = 0.4f; // ������ ������

    private float currentJumpReloadValue = 0f;
    private float chargedJumpDistance = 0; // �������� ����� ������� (� ��������� ������)

    private Vector2 mousePosition; // ���������� ����
    private Vector2 LastMousePosition; // ������ ���������� ����

    public bool isCanMove = true;
    public bool IsMoving = false; // ��������� ����� ����
    public bool IsButtonJumpPressed = false; // ��������� ��������� ������ ����
    public bool isPlayerHitEnemy = false; // ����� ��� ���������� �� ������� �������� � �������


    private void Start()
    {
        playerDash = GetComponent<PlayerDash>();
    }

    private void FixedUpdate()
    {
        CalculateJumpDistance();
        CalculateJumpReload();

        if (!isCanMove && IsMoving && !isPlayerHitEnemy && playerDash.isCanDash == false)
            MoveToTarget(LastMousePosition);// ���������� ���������
    }

    private void Update()
    {
        mousePosition = GetMouseWorldPosition();
        MouseChargingInput();
        JumpWhenMouseUP();
        StopMoveWhenCloseToPosition();
    }

    // ������� ������ ��� ������� ������� ��� LKM
    void MouseChargingInput()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0) && IsMoving == false && isCanMove == true)
        {
            IsButtonJumpPressed = true;
        }
    }

    // ���������� ��� ����� ������ � ��������� ������� �������
    void StopMoveWhenCloseToPosition()
    {
        var Direction = LastMousePosition - (Vector2)transform.position;

        if (Direction.magnitude < 0.1)
        { 
            IsMoving = false;
        }
    }

    // ���� ��������� LKM �� �������� ������
    void JumpWhenMouseUP()
    {
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0) && playerDash.isDashing == false && IsMoving == false && isCanMove == true)
        {
            playerDash.isCanDash = false;
            isCanMove = false;
            IsMoving = true;
            IsButtonJumpPressed = false;
            isPlayerHitEnemy = false;

            // ��������� ������ ����������� �� ������� ������� �� ������� ����
            Vector2 direction = (mousePosition - (Vector2)transform.position);

            // ������������ ����� ������� �� maxJumpDistance
            LastMousePosition = (Vector2)transform.position + Vector2.ClampMagnitude(direction, chargedJumpDistance);
            chargedJumpDistance = 0;
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

    // ����������� ������
    void CalculateJumpReload()
    {
        if (!IsMoving)
        {
            currentJumpReloadValue += Time.fixedDeltaTime;
            if (currentJumpReloadValue >= maxJumpReload)
            {
                currentJumpReloadValue = 0;
                isCanMove = true;
            }
        }
    }

    // ����� ����������� �� ������ �����
    void MoveToTarget(Vector2 LastMousePosition)
    {
        // ��������� ����������� ����� ������� ����������� � ���������� ���������
        transform.position = Vector2.MoveTowards(transform.position, LastMousePosition, moveSpeed * Time.deltaTime);
    }

    // �������, �� ������� ����� ���������� ����
    public Vector2 GetMouseWorldPosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    // ��������� ������ ����� �� ������������� � �����������
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            isPlayerHitEnemy = true;
            IsMoving = false;
        }

        if (collision.transform.tag == "barrel")
            IsMoving = false;
    }

}
