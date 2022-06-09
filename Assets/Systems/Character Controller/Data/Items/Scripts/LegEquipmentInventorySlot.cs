using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LegEquipmentInventorySlot : MonoBehaviour
{
    UIManager uiManager;

    public Image icon;
    LegEquipment item;

    private void Awake()
    {
        uiManager = GetComponentInParent<UIManager>();
    }

    public void AddItem(LegEquipment newItem)
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
        if (uiManager.legEquipmentSlotSelected)
        {
            if (uiManager.player.playerInventoryManager.currentLegEquipment != null)
            {
                uiManager.player.playerInventoryManager.legEquipmentInventory.Add(uiManager.player.playerInventoryManager.currentLegEquipment);
            }
            uiManager.player.playerInventoryManager.currentLegEquipment = item;
            uiManager.player.playerInventoryManager.legEquipmentInventory.Remove(item);
            uiManager.player.playerEquipmentManager.EquipAllEquipmentModels();
        }
        else
        {
            return;
        }

        uiManager.equipmentWindowUI.LoadArmorOnEquipmentScreen(uiManager.player.playerInventoryManager);
        uiManager.ResetAllSelectedSlots();
    }
}
