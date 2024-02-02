using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;

[System.Serializable]
public class InventorySystem 
{
    [SerializeField] private List<InventorySlot> inventorySlots;

    public List<InventorySlot> InventorySlots => inventorySlots;
    public int InventorySize => InventorySlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChanged;
    public event Action<SOItems> OnCurrentItemChangedInSystem;
    public event Action<SOItems> OnItemAddedToInventory;

    public InventorySystem(int size)
    {
        inventorySlots = new List<InventorySlot>(size);
        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    public bool AddToInventory(SOItems itemToAdd, int amountToAdd)
    {
        if(ContainsItem(itemToAdd, out List<InventorySlot> invSlot))
        {
            foreach(var slot in invSlot)
            {
                if(slot.RoomLeftInStack(amountToAdd))
                {
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    OnCurrentItemChangedInSystem?.Invoke(itemToAdd);
                    OnItemAddedToInventory?.Invoke(itemToAdd);
                    return true;
                }
            }
        }

        if(HasFreeSlot(out InventorySlot freeSlot))
        {
            freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
            OnInventorySlotChanged?.Invoke(freeSlot);
            return true;
        }
        return false;
    }

    public void RemoveItemsFromInventory(SOItems data, int amount)
    {
        if (ContainsItem(data, out List<InventorySlot> invSlot))
        {
            var lastSlot = invSlot.LastOrDefault();

            // Видаляємо кількість предметів зі стопки, що була запитана
            int itemsToRemove = Mathf.Min(lastSlot.StackSize, amount);
            lastSlot.RemoveFromStack(itemsToRemove);

            //// Віднімаємо вже видалені предмети з загальної кількості
            //amount -= itemsToRemove;
            if(lastSlot.StackSize <= 0)
                lastSlot.ClearSlot();

            //// Перевіряємо, чи залишились ще предмети в стопці
            //if (stackSize <= 0) 
            //{
            //    var lastSlotUI = InventoryDisplay.Instance.SlotDictionary.FirstOrDefault(slot => slot.Value == lastSlot);
            //    if(lastSlotUI.Key == null)
            //    {
            //        Debug.LogError("lastSlotUI.Key is null. Unable to update UI for removed item.");
            //    }

            //    lastSlotUI.Key.UpdateUISlot(lastSlot);
            //}

            // Вызываем OnInventorySlotChanged после завершения цикла
            OnInventorySlotChanged?.Invoke(lastSlot);
            OnCurrentItemChangedInSystem?.Invoke(data);
        }
    }


    public bool ContainsItem(SOItems itemToAdd, out List<InventorySlot> invSlot)
    {
        invSlot = InventorySlots.Where(i => i.ItemData == itemToAdd).ToList();
        return invSlot == null ? false : true;
    }

    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = InventorySlots.FirstOrDefault(i => i.ItemData == null);
        return freeSlot == null ? false : true;
    }
}
