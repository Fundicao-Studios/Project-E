using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableInventorySlot : MonoBehaviour
{
    PlayerInventoryManager playerInventory;
    ConsumableSlotManager consumableSlotManager;
    UIManager uiManager;
    public Image icon;
    ConsumableItem item;

    private void Awake()
    {
        playerInventory = FindObjectOfType<PlayerInventoryManager>();
        consumableSlotManager = FindObjectOfType<ConsumableSlotManager>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void AddItem(ConsumableItem newItem)
    {
        if (newItem != null)
        {
            item = newItem;
            icon.sprite = item.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }
        else
        {
            ClearInventorySlot();
        }
    }

    public void ClearInventorySlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void EquipThisItem()
    {
        if (uiManager.consumableSlot01Selected)
        {
            playerInventory.consumablesInventory.Add(playerInventory.consumablesInSlots[0]);
            playerInventory.consumablesInSlots[0] = item;
            playerInventory.consumablesInventory.Remove(item);
        }
        else if (uiManager.consumableSlot02Selected)
        {
            playerInventory.consumablesInventory.Add(playerInventory.consumablesInSlots[1]);
            playerInventory.consumablesInSlots[1] = item;
            playerInventory.consumablesInventory.Remove(item);
        }
        else
        {
            return;
        }

        if (playerInventory.currentConsumableIndex == - 1)
        {
            playerInventory.currentConsumable = playerInventory.consumablesInSlots[playerInventory.currentConsumableIndex + 1];
        }
        else
        {
            playerInventory.currentConsumable = playerInventory.consumablesInSlots[playerInventory.currentConsumableIndex];
        }

        consumableSlotManager.LoadConsumableOnSlot(playerInventory.currentConsumable);
    
        uiManager.equipmentWindowUI.LoadConsumableOnEquipmentScreen(playerInventory);
        uiManager.ResetAllSelectedSlots();
    }
}
