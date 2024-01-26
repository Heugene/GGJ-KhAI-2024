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
    private float DashCooldownTemp = 0;

    private Vector2 mousePosition; //  оординати миш≥

    public bool isCanDash = false; // может ли игрок сделать деш
    public bool isDashing = false; // делает ли игрок деш


    private void Start()
    {
        playerJump = GetComponent<PlayerJump>();

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

    // логика
    void FixedUpdate()
    {
        // ѕолучаем позицию мыши в мировых координатах
        mousePosition = playerJump.GetMouseWorldPosition();

        if(isCanDash)
            MakeDash();

        if (isDashing)
        {
            CalculateDash();
            CalculateDashReload();
        }
    }

    // input процесы
    void Update()
    {
        // ѕолучаем позицию мыши в мировых координатах
        mousePosition = playerJump.GetMouseWorldPosition();

        // если отпустить RKM и выбран соус то начинаем деш
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

    // убрать предмет из слота
    void RemoveCurrentItemInSlot(SOItems currentItem)
    {
        InventorySlot_UI currentSlotUI = inventoryDisplay.SlotDictionary.FirstOrDefault(slot => slot.Value.ItemData == currentItem).Key;

        if (currentSlotUI != null && currentSlotUI.AssignedInventorySlot.StackSize == 0)
        {
            currentSlotUI.ClearSlot();
            currentItemType = ItemType.None;
        }
    }

    // уменьшение скорости деша на заданое значение DashSpeedReducer
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

    // перезар€дка деша в секундах
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

    // —оздание деша
    void MakeDash()
    {
        // ¬ычисл€ем вектор направлени€ от текущей позиции до позиции мыши
        Vector2 direction = mousePosition - (Vector2)transform.position;

        // ѕримен€ем MoveTowards дл€ движени€ к курсору
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + direction.normalized, DashSpeed * Time.deltaTime);
    }
}