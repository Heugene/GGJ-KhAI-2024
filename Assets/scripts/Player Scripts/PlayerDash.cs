using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public bool isDashing = false;  // Позначає, чи виконує гравець деш
    public bool isFreezed = false;  // Вказує, чи зупинена логіка скрипта

    [SerializeField] private float DashSpeed = 12;              // Швидкість дешу
    [SerializeField] private float DashSpeedReducer = 0.05f;    // Зменшення швидкості дешу
    [SerializeField] private ItemType currentItemType;          // Поточний тип предмету для дешу
    [SerializeField] private List<ItemType> itemTypesForDash;   // Список типів предметів, які можна використовувати для дешу

    private PlayerJump playerJump;                  // Зовнішній компонент гравця для використання в деші
    private InventoryDisplay inventoryDisplay;      // Відображення інвентаря для управління предметами
    private TrailRenderer trailRendererKetchup;     // Слід за гравцем для типу предмету Кетчуп
    private TrailRenderer trailRendererMayonnaise;  // Слід за гравцем для типу предмету Майонез

    private Vector2 mousePosition;   // Координати миші
    private float InitialDashSpeed;  // Початкова швидкысть дешу


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

    // Пошук TrailRenderer об'єкту із вказаним ім'ям
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

    // Виконання дій
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

    // Считування Input процесів
    private void Update()
    {
        if (isFreezed) return;

        mousePosition = playerJump.GetMouseWorldPosition();

        if (CanStartDash())
        {
            StartDash();
        }
    }

    // Зменшення швидкості деша на вказане значення DashSpeedReducer
    private void CalculateDash()
    {
        DashSpeed -= DashSpeedReducer;
        if (DashSpeed <= 0)
        {
            StopDash();
        }
    }


    // Переміщення гравця до позиції миші під час дешу
    private void MoveToMouse()
    {
        Vector2 direction = mousePosition - (Vector2)transform.position;

        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + direction.normalized, DashSpeed * Time.deltaTime);
    }

    // Розпочаток дешу
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

    // Активація слідів в залежності від типу предмету
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

    // Завершення дешу
    private void StopDash()
    {
        isDashing = false;
        EndDashSound();
        DeactivateTrails();
        DashSpeed = InitialDashSpeed;
    }

    // Відтворення звуку дешу
    private void PlayDashSound() => ControlDashSound(true);

    // Завершення звуку дешу
    private void EndDashSound() => ControlDashSound(false);

    // Керує звуком дешу, відтворюючи або зупиняючи його
    // в залежності від параметру isPlaying.
    private void ControlDashSound(bool isPlaying)
    {
        playerJump.audioSource.clip = playerJump.sausageSounds[1];
        playerJump.audioSource.loop = isPlaying;

        if (isPlaying)
            playerJump.audioSource.Play();
        else
            playerJump.audioSource.Stop();
    }

    // Вимкнення слідів
    private void DeactivateTrails()
    {
        trailRendererKetchup.emitting = false;
        trailRendererMayonnaise.emitting = false;
    }

    // Перевіряє можливість початку дії дешу
    private bool CanStartDash()
    {
        return Input.GetKeyUp(KeyCode.Mouse1) && itemTypesForDash.Contains(currentItemType) && !isDashing;
    }

    // Обробник зміни поточного предмету
    private void HandleCurrentItemChanged(SOItems newItem) => currentItemType = newItem?.ItemType ?? ItemType.None;

    // Видалення поточного предмету із слоту
    private void RemoveCurrentItemInSlot() => currentItemType = ItemType.None;
}