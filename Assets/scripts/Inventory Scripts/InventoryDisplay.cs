using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.Events;

public abstract class InventoryDisplay : MonoBehaviour
{
    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionary;
    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;
    protected int selectedSlotIndex = 0; 
    public UnityEvent<int> OnSelectedSlotChanged = new UnityEvent<int>();

    protected virtual void Start()
    {

    }
    private void Update()
    {
        float scrollDelta = Input.mouseScrollDelta.y;
        if (scrollDelta != 0)
        {
            ChangeSelectedSlot(scrollDelta);
        }
    }
    private void Awake()
    {
        OnSelectedSlotChanged.AddListener(OnSelectedSlotChange);
    }
    private void OnSelectedSlotChange(int newIndex)
    {
        //Debug.Log("Selected Slot Index: " + newIndex);

        foreach (var slot in SlotDictionary)
        {
            InventorySlot_UI slotUI = slot.Key;
            InventorySlot inventorySlot = slot.Value;
            if (slotUI != null)
            {
                slotUI.SetSelected(newIndex == slotUI.transform.GetSiblingIndex());
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