using System.Collections.Generic;
using System.Linq;
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
    private ItemType currentItemType;
    [SerializeField]
    private List<ItemType> ItemTypeForDash;
    private InventoryDisplay inventoryDisplay;
    private TrailRenderer trailRenderer;
    private TrailRenderer trailRendererMayonnaise;


    private Vector2 mousePosition; // ���������� ����
    private Vector2 LastMousePosition; // ������ ���������� ����
    public bool IsMoving = false; // ��������� ����� ����
    public bool IsButtonJumpPressed = false; // ��������� ��������� ������ ����
    public bool isPlayerHitEnemy = false; // ����� ��� ���������� �� ������� �������� � �������
    public bool isCanDash = false; // ����� �� ����� ������� ���
    public bool isDashing = false; // ������ �� ����� ���


    private void Start()
    {
        DashSpeedTemp = DashSpeed;

        inventoryDisplay = InventoryDisplay.Instance;

        if (inventoryDisplay != null)
        {
            inventoryDisplay.OnCurrentItemChanged += HandleCurrentItemChanged;
        }
        else
        {
            Debug.LogError("InventoryDisplay not found.");
        }
        Transform trailObject = transform.Find("Trail");
        Transform trailObject1 = transform.Find("Mayonnaise");
        if (trailObject != null || trailObject1 != null)
        {
            trailRenderer = trailObject.GetComponent<TrailRenderer>();
            trailRendererMayonnaise = trailObject1.GetComponent<TrailRenderer>();
            if (trailRenderer != null || trailRendererMayonnaise != null)
            {
                trailRenderer.emitting = false;
                trailRendererMayonnaise.emitting = false;
            }
            else
            {
                Debug.LogError("TrailRenderer not found on the child object with the 'Trail' tag.");
            }
        }
        else
        {
            Debug.LogError("Child object with the 'Trail' tag not found.");
        }
    }
    private void HandleCurrentItemChanged(SOItems newItem)
    {
        if (newItem == null || newItem.ItemType == null)
        {
            currentItemType = ItemType.None;
        }
        else
        {
            currentItemType = newItem.ItemType;
        }
    }


    // ������
    void FixedUpdate()
    {
        // �������� ������� ���� � ������� �����������
        mousePosition = GetMouseWorldPosition();

        if(isCanDash)
            MakeDash();

        if (isDashing)
        {
            CalculateDash();
            CalculateDashReload();
        }

        CalculateJumpDistance();

        if (IsMoving && !isPlayerHitEnemy && isCanDash == false)
            MoveToTarget(LastMousePosition);// ���������� ���������
    }

    // input �������
    void Update()
    {
        // �������� ������� ���� � ������� �����������
        mousePosition = GetMouseWorldPosition();

        // ������� ������ ��� ������� ������� ��� LKM
        MouseChargingInput();

        // ���� ��������� LKM �� �������� ������
        JumpWhenMouseUP();

        // ����� ����� ������ � ����� �� ����������� Dash
        StopDashWhenCloseToPosition();

        // ���� ��������� RKM � ������ ���� �� �������� ���
        StartDash();
    }

    void StartDash()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1) && ItemTypeForDash.Contains(currentItemType) && !isDashing)
        {
            isCanDash = true;
            isDashing = true;

            SOItems currentItem = inventoryDisplay.GetCurrentItem();

            if (currentItem != null)
            {
                if (currentItemType == ItemType.Ketchup)
                    trailRenderer.emitting = true;
                else if (currentItemType == ItemType.Mayonnaise)
                    trailRendererMayonnaise.emitting = true;

                inventoryDisplay.InventorySystem.RemoveItemsFromInventory(currentItem, 1);
                RemoveCurrentItemInSlot(currentItem);
            }
        }
    }

    // ������� ������ ��� ������� ������� ��� LKM
    void MouseChargingInput()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0))
        {
            IsMoving = false;
            IsButtonJumpPressed = true;
        }
    }

    // ���� ��������� LKM �� �������� ������
    void JumpWhenMouseUP()
    {
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
    }

    // ���������� ��� ����� ������ � ��������� ������� �������
    void StopDashWhenCloseToPosition()
    {
        var Direction = LastMousePosition - (Vector2)transform.position;
        if (Direction.magnitude < 0.1)
        {
            IsMoving = false;
        }
    }

    // ������ ������� �� �����
    void RemoveCurrentItemInSlot(SOItems currentItem)
    {
        InventorySlot_UI currentSlotUI = inventoryDisplay.SlotDictionary.FirstOrDefault(slot => slot.Value.ItemData == currentItem).Key;

        if (currentSlotUI != null && currentSlotUI.AssignedInventorySlot.StackSize == 0)
        {
            currentSlotUI.ClearSlot();
            currentItemType = ItemType.None;
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
        DashSpeed -= DashSpeedReducer;
        if(DashSpeed <= 0)
        {
            isCanDash = false;
            isDashing = false;
            trailRenderer.emitting = false;
            trailRendererMayonnaise.emitting = false;
            DashSpeed = DashSpeedTemp;
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
                isCanDash = true;
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
    public Vector2 GetMouseWorldPosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    // ��������� ������ ����� �� ������������� � �����������
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
            isPlayerHitEnemy = true;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
            isPlayerHitEnemy = true;
    }
}