using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 7f; // Швидкість польоту у стрибку
    [SerializeField]
    private float maxJumpDistance = 5f; // Максимальна дальність стрибку
    private float chargedJumpDistance = 0; // Поточний заряд стрибку (У еквіваленті відстані)
    [SerializeField]
    private float chargedJumpDistanceMultiplier = 0.4f; // Приріст заряду
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


    private Vector2 mousePosition; // Координати миші
    private Vector2 LastMousePosition; // Останні координати миші
    public bool IsMoving = false; // Прапорець стану руху
    public bool IsButtonJumpPressed = false; // Прапорець утримання кнопки руху
    public bool isPlayerHitEnemy = false; // змінна для визначення чи гравець зіткнувся з ворогом
    public bool isCanDash = false; // может ли игрок сделать деш
    public bool isDashing = false; // делает ли игрок деш


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


    // логика
    void FixedUpdate()
    {
        // Получаем позицию мыши в мировых координатах
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
            MoveToTarget(LastMousePosition);// Перемещаем персонажа
    }

    // input процесы
    void Update()
    {
        // Получаем позицию мыши в мировых координатах
        mousePosition = GetMouseWorldPosition();

        // зарядка прыжка при зажатом пробеле или LKM
        MouseChargingInput();

        // Если отпустить LKM то начинаем прыжок
        JumpWhenMouseUP();

        // когда очень близко к точке то заканчиваем Dash
        StopDashWhenCloseToPosition();

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

    // зарядка прыжка при зажатом пробеле или LKM
    void MouseChargingInput()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0))
        {
            IsMoving = false;
            IsButtonJumpPressed = true;
        }
    }

    // Если отпустить LKM то начинаем прыжок
    void JumpWhenMouseUP()
    {
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0) && isDashing == false)
        {
            isCanDash = false;
            IsMoving = true;
            IsButtonJumpPressed = false;
            isPlayerHitEnemy = false;

            // Вычисляем вектор направления от текущей позиции до позиции мыши
            Vector2 direction = (mousePosition - (Vector2)transform.position);

            // Ограничиваем длину вектора до maxJumpDistance
            LastMousePosition = (Vector2)transform.position + Vector2.ClampMagnitude(direction, chargedJumpDistance);
            chargedJumpDistance = 0;
        }
    }

    // остановить деш когда близко к последней позиции курсора
    void StopDashWhenCloseToPosition()
    {
        var Direction = LastMousePosition - (Vector2)transform.position;
        if (Direction.magnitude < 0.1)
        {
            IsMoving = false;
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

    // зарядка прыжка
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

    // перезарядка деша в секундах
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

    // Метод пересування до заданої точки
    void MoveToTarget(Vector2 LastMousePosition)
    {
        // Применяем перемещение вдоль вектора направления с постоянной скоростью
        transform.position = Vector2.MoveTowards(transform.position, LastMousePosition, moveSpeed * Time.deltaTime);
    }

    // Создание деша
    void MakeDash()
    {
        // Вычисляем вектор направления от текущей позиции до позиции мыши
        Vector2 direction = mousePosition - (Vector2)transform.position;

        // Применяем MoveTowards для движения к курсору
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + direction.normalized, DashSpeed * Time.deltaTime);

        // Обновляем LastMousePosition
        LastMousePosition = transform.position;
    }

    // Функція, що повертає світові координати миші
    public Vector2 GetMouseWorldPosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    // остановка игрока когда он сталкиваеться с противником
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