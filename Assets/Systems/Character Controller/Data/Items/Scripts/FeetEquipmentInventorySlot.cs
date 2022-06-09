using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeetEquipmentInventorySlot : MonoBehaviour
{
    UIManager uiManager;

    public Image icon;
    FeetEquipment item;

    private void Awake()
    {
        uiManager = GetComponentInParent<UIManager>();
    }

    public void AddItem(FeetEquipment newItem)
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
        if (uiManager.feetEquipmentSlotSelected)
        {
            if (uiManager.player.playerInventoryManager.currentFeetEquipment != null)
            {
                uiManager.player.playerInventoryManager.feetEquipmentInventory.Add(uiManager.player.playerInventoryManager.currentFeetEquipment);
            }
            uiManager.player.playerInventoryManager.currentFeetEquipment = item;
            uiManager.player.playerInventoryManager.feetEquipmentInventory.Remove(item);
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
