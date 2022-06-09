using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeetEquipmentSlotUI : MonoBehaviour
{
    UIManager uiManager;

    public Image icon;
    FeetEquipment item;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    public void AddItem(FeetEquipment feetEquipment)
    {
        if(feetEquipment !=null)
        {
            item = feetEquipment;
            icon.sprite = item.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }
        else
        {
            ClearItem();
        }
    }

    public void ClearItem()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void SelectThisSlot()
    {
        uiManager.itemStatsWindow.SetActive(true);
        uiManager.feetEquipmentSlotSelected = true;
        uiManager.itemStatsWindowUI.UpdateArmorItemStats(item);
    }

    public void UnselectThisSlot()
    {
        uiManager.itemStatsWindow.SetActive(false);
    }
}
