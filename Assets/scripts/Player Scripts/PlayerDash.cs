using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public bool isDashing = false;  // �������, �� ������ ������� ���
    public bool isFreezed = false;  // �����, �� �������� ����� �������

    [SerializeField] private float DashSpeed = 12;              // �������� ����
    [SerializeField] private float DashSpeedReducer = 0.05f;    // ��������� �������� ����
    [SerializeField] private ItemType currentItemType;          // �������� ��� �������� ��� ����
    [SerializeField] private List<ItemType> itemTypesForDash;   // ������ ���� ��������, �� ����� ��������������� ��� ����

    private PlayerJump playerJump;                  // ������� ��������� ������ ��� ������������ � ����
    private InventoryDisplay inventoryDisplay;      // ³���������� ��������� ��� ��������� ����������
    private TrailRenderer trailRendererKetchup;     // ��� �� ������� ��� ���� �������� ������
    private TrailRenderer trailRendererMayonnaise;  // ��� �� ������� ��� ���� �������� �������

    private Vector2 mousePosition;   // ���������� ����
    private float InitialDashSpeed;  // ��������� ��������� ����


    private void Start()
    {
        playerJump = GetComponent<PlayerJump>();
        inventoryDisplay = InventoryDisplay.Instance;

        if (inventoryDisplay != null)
        {
            inventoryDisplay.OnCurrentItemChanged += HandleCurrentItemChanged;
            inventoryDisplay.InventorySystem.OnStackCleared += RemoveCurrentItemInSlot;
        }
        else
        {
            Debug.LogError("InventoryDisplay not found.");
        }

        InitialDashSpeed = DashSpeed;

        trailRendererKetchup = FindTrailRendererByName("Trail");
        trailRendererMayonnaise = FindTrailRendererByName("Mayonnaise");

        trailRendererKetchup.emitting = false;
        trailRendererMayonnaise.emitting = false;
    }

    // ����� TrailRenderer ��'���� �� �������� ��'��
    public TrailRenderer FindTrailRendererByName(string objectName)
    {
        Transform trailObject = transform.Find(objectName);

        if (trailObject == null)
        {
            Debug.LogError($"Child object with the specified name '{objectName}' not found.");
            return null;
        }

        TrailRenderer trailRenderer = trailObject.GetComponent<TrailRenderer>();

        if (trailRenderer == null)
        {
            Debug.LogError("TrailRenderer not found on the child object with the specified name.");
            return null;
        }

        return trailRenderer;
    }

    // ��������� ��
    private void FixedUpdate()
    {
        if (isFreezed) return;

        mousePosition = playerJump.GetMouseWorldPosition();

        if (isDashing)
        {
            CalculateDash();
            MoveToMouse();
        }
    }

    // ���������� Input �������
    private void Update()
    {
        if (isFreezed) return;

        mousePosition = playerJump.GetMouseWorldPosition();

        if (CanStartDash())
        {
            StartDash();
        }
    }

    // ��������� �������� ���� �� ������� �������� DashSpeedReducer
    private void CalculateDash()
    {
        DashSpeed -= DashSpeedReducer;
        if (DashSpeed <= 0)
        {
            StopDash();
        }
    }


    // ���������� ������ �� ������� ���� �� ��� ����
    private void MoveToMouse()
    {
        Vector2 direction = mousePosition - (Vector2)transform.position;

        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + direction.normalized, DashSpeed * Time.deltaTime);
    }

    // ���������� ����
    private void StartDash()
    {
        PlayDashSound();
        playerJump.IsMoving = false;
        isDashing = true;

        SOItems currentItem = inventoryDisplay.GetCurrentItem();

        if (currentItem != null)
        {
            ActivateTrail(currentItemType);
            inventoryDisplay.InventorySystem.RemoveItemsFromInventory(currentItem, 1);
        }
    }

    // ��������� ���� � ��������� �� ���� ��������
    private void ActivateTrail(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Ketchup:
                trailRendererKetchup.emitting = true;
                break;
            case ItemType.Mayonnaise:
                trailRendererMayonnaise.emitting = true;
                break;

            default: 
                break;
        }
    }

    // ���������� ����
    private void StopDash()
    {
        isDashing = false;
        EndDashSound();
        DeactivateTrails();
        DashSpeed = InitialDashSpeed;
    }

    // ³��������� ����� ����
    private void PlayDashSound() => ControlDashSound(true);

    // ���������� ����� ����
    private void EndDashSound() => ControlDashSound(false);

    // ���� ������ ����, ���������� ��� ��������� ����
    // � ��������� �� ��������� isPlaying.
    private void ControlDashSound(bool isPlaying)
    {
        playerJump.audioSource.clip = playerJump.sausageSounds[1];
        playerJump.audioSource.loop = isPlaying;

        if (isPlaying)
            playerJump.audioSource.Play();
        else
            playerJump.audioSource.Stop();
    }

    // ��������� ����
    private void DeactivateTrails()
    {
        trailRendererKetchup.emitting = false;
        trailRendererMayonnaise.emitting = false;
    }

    // �������� ��������� ������� 䳿 ����
    private bool CanStartDash()
    {
        return Input.GetKeyUp(KeyCode.Mouse1) && itemTypesForDash.Contains(currentItemType) && !isDashing;
    }

    // �������� ���� ��������� ��������
    private void HandleCurrentItemChanged(SOItems newItem) => currentItemType = newItem?.ItemType ?? ItemType.None;

    // ��������� ��������� �������� �� �����
    private void RemoveCurrentItemInSlot() => currentItemType = ItemType.None;
}