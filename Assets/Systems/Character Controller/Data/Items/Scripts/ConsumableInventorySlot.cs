using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableInventorySlot : MonoBehaviour
{
    PlayerInventory playerInventory;
    ConsumableSlotManager consumableSlotManager;
    UIManager uiManager;
    public Image icon;
    ConsumableItem item;

    private void Awake()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        consumableSlotManager = FindObjectOfType<ConsumableSlotManager>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void AddItem(ConsumableItem newItem)
    {
        item = newItem;
        icon.sprite = item.itemIcon;
        icon.enabled = true;
        gameObject.SetActive(true);
    }

    public void ClearInventorySlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        gameObject.SetActive(false);
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

        playerInventory.currentConsumable = playerInventory.consumablesInSlots[playerInventory.currentConsumableIndex];

        consumableSlotManager.LoadConsumableOnSlot(playerInventory.currentConsumable);
    
        uiManager.equipmentWindowUI.LoadConsumableOnEquipmentScreen(playerInventory);
        uiManager.ResetAllSelectedSlots();
    }
}
