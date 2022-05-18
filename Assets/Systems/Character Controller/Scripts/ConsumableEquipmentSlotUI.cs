using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableEquipmentSlotUI : MonoBehaviour
{
    UIManager uiManager;

    public Image icon;
    ConsumableItem potion;

    public bool consumableSlot01;
    public bool consumableSlot02;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    public void AddItem(ConsumableItem newPotion)
    {
        potion = newPotion;
        icon.sprite = potion.itemIcon;
        icon.enabled = true;
        gameObject.SetActive(true);
    }

    public void ClearItem()
    {
        potion = null;
        icon.sprite = null;
        icon.enabled = false;
        gameObject.SetActive(false);
    }

    public void SelectThisSlot()
    {
        if (consumableSlot01)
        {
            uiManager.consumableSlot01Selected = true;
        }
        else if (consumableSlot02)
        {
            uiManager.consumableSlot02Selected = true;
        }
    }
}
