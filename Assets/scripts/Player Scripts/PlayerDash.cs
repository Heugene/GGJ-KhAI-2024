using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    PlayerJump playerJump;

    [SerializeField] private float DashSpeed = 12;
    [SerializeField] private float DashCooldown = 1;
    [SerializeField] private float DashSpeedReducer = 0.05f;

    [SerializeField] private ItemType currentItemType;
    [SerializeField] private List<ItemType> ItemTypeForDash;

    private InventoryDisplay inventoryDisplay;
    private TrailRenderer trailRenderer;
    private TrailRenderer trailRendererMayonnaise;

    private float DashSpeedTemp = 0;
    private float DashCooldownTemp = 0; //#CAN BE MOVED IN TRASH

    private Vector2 mousePosition; // ���������� ����

    public bool isCanDash = false; // ����� �� ����� ������� ���
    public bool isDashing = false; // ������ �� ����� ���
    public bool isFreezed = false; // ������������� ������ �������


    private void Start()
    {
        playerJump = GetComponent<PlayerJump>();

        DashSpeedTemp = DashSpeed;

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

        trailRenderer = FindTrailRendererByName("Trail");
        trailRendererMayonnaise = FindTrailRendererByName("Mayonnaise");

        trailRenderer.emitting = false;
        trailRendererMayonnaise.emitting = false;
    }

    // ���� TrailRenderer ��'���� �� �������� ��'��
    public TrailRenderer FindTrailRendererByName(string objectName)
    {
        Transform trailObject = transform.Find(objectName);

        if (trailObject != null)
        {
            TrailRenderer trailRenderer = trailObject.GetComponent<TrailRenderer>();

            if (trailRenderer != null)
            {
                return trailRenderer;
            }
            else
            {
                Debug.LogError("TrailRenderer not found on the child object with the specified name.");
            }
        }
        else
        {
            Debug.LogError($"Child object with the specified name '{objectName}' not found.");
        }

        return null; // ��������� null � ������� �������
    }

    private void HandleCurrentItemChanged(SOItems newItem)
    {
        if (newItem == null)
        {
            currentItemType = ItemType.None;
        }
        else
        {
            currentItemType = newItem.ItemType;
        }
    }

    // ������ ������� �� �����
    void RemoveCurrentItemInSlot()
    {
        currentItemType = ItemType.None;
    }

    // ������
    void FixedUpdate()
    {
        if (isFreezed) return;

        // �������� ������� ���� � ������� �����������
        mousePosition = playerJump.GetMouseWorldPosition();

        if(isCanDash)   // #CAN BE MOVED IN TRASH
            MakeDash(); //

        if (isDashing)
        {
            CalculateDash();
            CalculateDashReload();
        }
    }

    // input �������
    void Update()
    {
        if (isFreezed) return;

        // �������� ������� ���� � ������� �����������
        mousePosition = playerJump.GetMouseWorldPosition();

        // ���� ��������� RKM � ������ ���� �� �������� ���
        StartDash();
    }

    void StartDash()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1) && ItemTypeForDash.Contains(currentItemType) && !isDashing && !playerJump.IsMoving)
        {
            playerJump.audioSource.clip = playerJump.sausageSounds[1];
            playerJump.audioSource.loop = true;
            playerJump.audioSource.Play();

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
            }
        }
    }

    // ���������� �������� ���� �� ������� �������� DashSpeedReducer
    void CalculateDash()
    {
        DashSpeed -= DashSpeedReducer;
        if (DashSpeed <= 0)
        {
            playerJump.audioSource.Stop();
            playerJump.audioSource.loop = false;

            isCanDash = false;
            isDashing = false;

            trailRenderer.emitting = false;
            trailRendererMayonnaise.emitting = false;
            DashSpeed = DashSpeedTemp;
        }
    }

    // ����������� ���� � ��������
    void CalculateDashReload()                        // #CAN BE MOVED IN TRASH
    {                                                 //
        if (!isCanDash)                               //
        {                                             //
            DashCooldownTemp += Time.fixedDeltaTime;  //
            if (DashCooldownTemp >= DashCooldown)     //
            {                                         //
                DashCooldownTemp = 0;                 //
                isCanDash = true;                     //
            }                                         //
        }                                             //
    }                                                 //

    // �������� ����
    void MakeDash()
    {
        // ��������� ������ ����������� �� ������� ������� �� ������� ����
        Vector2 direction = mousePosition - (Vector2)transform.position;

        // ��������� MoveTowards ��� �������� � �������
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + direction.normalized, DashSpeed * Time.deltaTime);
    }
}