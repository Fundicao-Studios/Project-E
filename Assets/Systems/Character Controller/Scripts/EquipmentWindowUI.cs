using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentWindowUI : MonoBehaviour
{
    public bool rightHandSlot01Selected;
    public bool rightHandSlot02Selected;
    public bool leftHandSlot01Selected;
    public bool leftHandSlot02Selected;
    public bool consumableSlot01Selected;
    public bool consumableSlot02Selected;

    public HandEquipmentSlotUI[] handEquipmentSlotUI;
    public ConsumableEquipmentSlotUI[] consumableEquipmentSlotUI;

    public void LoadWeaponOnEquipmentScreen(PlayerInventoryManager playerInventory)
    {
        for (int i = 0; i < handEquipmentSlotUI.Length; i++)
        {
            if (handEquipmentSlotUI[i].rightHandSlot01)
            {
                handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[0]);
            }
            else if (handEquipmentSlotUI[i].rightHandSlot02)
            {
                handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[1]);
            }
            else if (handEquipmentSlotUI[i].leftHandSlot01)
            {
                handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[0]);
            }
            else
            {
                handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[1]);
            }
        }
    }

    public void LoadConsumableOnEquipmentScreen(PlayerInventoryManager playerInventory)
    {
        for (int i = 0; i < consumableEquipmentSlotUI.Length; i++)
        {
            if (consumableEquipmentSlotUI[i].consumableSlot01)
            {
                consumableEquipmentSlotUI[i].AddItem(playerInventory.consumablesInSlots[0]);
            }
            else if (consumableEquipmentSlotUI[i].consumableSlot02)
            {
                consumableEquipmentSlotUI[i].AddItem(playerInventory.consumablesInSlots[1]);
            }
        }
    }

    public void SelectRightHandSlot01()
    {
        rightHandSlot01Selected = true;
    }

    public void SelectRightHandSlot02()
    {
        rightHandSlot02Selected = true;
    }

    public void SelectLeftHandSlot01()
    {
        leftHandSlot01Selected = true;
    }

    public void SelectLeftHandSlot02()
    {
        leftHandSlot02Selected = true;
    }

    public void SelectConsumableSlot01()
    {
        consumableSlot01Selected = true;
    }

    public void SelectConsumableSlot02()
    {
        consumableSlot02Selected = true;
    }
}
