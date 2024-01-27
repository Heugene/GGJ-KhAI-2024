using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public abstract class InventoryDisplay : MonoBehaviour
{
    public static InventoryDisplay Instance { get; private set; }
    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionary;
    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;
    protected int selectedSlotIndex = -2; 
    public UnityEvent<int> OnSelectedSlotChanged = new UnityEvent<int>();
    [SerializeField]private SOItems currentItem;
    public event Action<SOItems> OnCurrentItemChanged;
    protected bool hasStarted = false;

    public void SetCurrentItem(SOItems newItem)
    {
        currentItem = newItem;

        // Вызов события для уведомления других скриптов о изменении текущего элемента
        OnCurrentItemChanged?.Invoke(currentItem);
    }
    protected virtual void Start()
    {

    }
    private void Update()
    {
        if (!hasStarted)
        {
            ChangeSelectedSlot(1); // Индекс первого слота, который вы хотите выбрать
            hasStarted = true;
        }
        float scrollDelta = Input.mouseScrollDelta.y;
        if (scrollDelta != 0)
        {
            ChangeSelectedSlot(scrollDelta);
        }
        OnSelectedSlotChange(selectedSlotIndex);

    }
    public InventorySlot_UI GetCurrentSlot()
    {
        if (selectedSlotIndex >= 0 && selectedSlotIndex < slotDictionary.Count)
        {
            return slotDictionary.ElementAt(selectedSlotIndex).Key;
        }
        return null;
    }
    public SOItems GetCurrentItem()
    {
        return currentItem;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        OnSelectedSlotChanged.AddListener(OnSelectedSlotChange);
        if (inventorySystem != null)
        {
            inventorySystem.OnCurrentItemChangedInSystem += HandleCurrentItemChangedInSystem;
        }
    }
    private void HandleCurrentItemChangedInSystem(SOItems newItem)
    {
        SetCurrentItem(newItem);
    }
     private void OnSelectedSlotChange(int newIndex)
     {
         foreach (var slot in SlotDictionary)
         {
             InventorySlot_UI slotUI = slot.Key;
             InventorySlot inventorySlot = slot.Value;

             if (slotUI != null)
             {
                 bool isSelected = newIndex == slotUI.transform.GetSiblingIndex();
                 slotUI.SetSelected(isSelected);

                 if (isSelected)
                 {
                     if (inventorySlot != null && inventorySlot.ItemData != null)
                     {
                         SetCurrentItem(inventorySlot.ItemData);

                    }
                    else
                     {
                         // Если предмет отсутствует, присвоить ItemType.None
                         SetCurrentItem(null);
                     }
                 }
             }
         }
     }

    protected void ChangeSelectedSlot(float delta)
    {
        if (delta < 0)
        {
            selectedSlotIndex = (selectedSlotIndex + 1) % slotDictionary.Count;
        }
        else
        {
            selectedSlotIndex = (selectedSlotIndex - 1 + slotDictionary.Count) % slotDictionary.Count;
        }
        OnSelectedSlotChanged.Invoke(selectedSlotIndex);
        UpdateSlotVisibility();
        //Debug.Log($"ItemData in selected slot after scroll: {slotDictionary.Values.ElementAt(selectedSlotIndex).ItemData}");
    }

    protected void UpdateSlotVisibility()
    {
        foreach (var slot in SlotDictionary)
        {
            InventorySlot_UI slotUI = slot.Key;
            InventorySlot inventorySlot = slot.Value;

        }
    }
    public abstract void AssignSlot(InventorySystem invToDisplay, int offset);

    protected void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in SlotDictionary)
        {
            if (slot.Value == updatedSlot)
            {
                slot.Key.UpdateUISlot(updatedSlot);
            }
        }

    }


    public void SlotClicked(InventorySlot_UI clickedUISlot)
    {

    }
}