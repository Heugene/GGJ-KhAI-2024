using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot_UI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private InventorySlot assignedInventorySlot;
    [SerializeField] private Image selectedIndicator;

    private Button button;
    private bool isSelected = false;

    public InventorySlot AssignedInventorySlot => assignedInventorySlot;
    public InventoryDisplay ParentDisplay { get; private set; }

    private void Awake()
    {
        ClearSlot();
        itemSprite.preserveAspect = true;
        button = GetComponent<Button>();
        button?.onClick.AddListener(OnUISlotClick);
        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
    }

    public void Init(InventorySlot slot)
    {
        assignedInventorySlot = slot;
        UpdateUISlot(slot);
        SetSelected(false);
    }

    public void SetSelected(bool isSelected)
    {
        this.isSelected = isSelected;

        if (selectedIndicator != null)
        {
            Image imageComponent = selectedIndicator.GetComponent<Image>();
            if (imageComponent != null)
            {
                imageComponent.enabled = isSelected;
            }
        }

        UpdateUISlot();
    }

    public void UpdateUISlot(InventorySlot slot)
    {
        if (slot.ItemData != null)
        {
            itemSprite.sprite = slot.ItemData.ItemImage;
            itemSprite.color = Color.white;

            if (slot.StackSize > 1) itemCount.text = slot.StackSize.ToString();
            else itemCount.text = "";

        }
        else
        {
            ClearSlot();
        }

        if (selectedIndicator != null)
        {
            selectedIndicator.gameObject.SetActive(isSelected);
        }
    }

    public void UpdateUISlot()
    {
        if (assignedInventorySlot != null)
        {
            UpdateUISlot(assignedInventorySlot);
        }
    }

    public void ClearSlot()
    {
        assignedInventorySlot?.ClearSlot();
        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = "";
    }

    public void OnUISlotClick()
    {
        isSelected = !isSelected;
        UpdateUISlot();
        ParentDisplay?.SlotClicked(this);
    }
}
