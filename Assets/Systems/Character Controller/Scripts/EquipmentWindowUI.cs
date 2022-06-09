using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentWindowUI : MonoBehaviour
{
    public WeaponEquipmentSlotUI[] weaponEquipmentSlotUI;
    public HeadEquipmentSlotUI headEquipmentSlotUI;
    public BodyEquipmentSlotUI bodyEquipmentSlotUI;
    public LegEquipmentSlotUI legEquipmentSlotUI;
    public FeetEquipmentSlotUI feetEquipmentSlotUI;
    public ConsumableEquipmentSlotUI[] consumableEquipmentSlotUI;

    public void LoadWeaponOnEquipmentScreen(PlayerInventoryManager playerInventory)
    {
        for (int i = 0; i < weaponEquipmentSlotUI.Length; i++)
        {
            if (weaponEquipmentSlotUI[i].rightHandSlot01)
            {
                weaponEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[0]);
            }
            else if (weaponEquipmentSlotUI[i].rightHandSlot02)
            {
                weaponEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[1]);
            }
            else if (weaponEquipmentSlotUI[i].leftHandSlot01)
            {
                weaponEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[0]);
            }
            else
            {
                weaponEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[1]);
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

    public void LoadArmorOnEquipmentScreen(PlayerInventoryManager playerInventoryManager)
    {
        if (playerInventoryManager.currentHelmetEquipment != null)
        {
            headEquipmentSlotUI.AddItem(playerInventoryManager.currentHelmetEquipment);
        }
        else
        {
            headEquipmentSlotUI.ClearItem();
        }

        if (playerInventoryManager.currentBodyEquipment != null)
        {
            bodyEquipmentSlotUI.AddItem(playerInventoryManager.currentBodyEquipment);
        }
        else
        {
            bodyEquipmentSlotUI.ClearItem();
        }

        if (playerInventoryManager.currentLegEquipment != null)
        {
            legEquipmentSlotUI.AddItem(playerInventoryManager.currentLegEquipment);
        }
        else
        {
            legEquipmentSlotUI.ClearItem();
        }

        if (playerInventoryManager.currentFeetEquipment != null)
        {
            feetEquipmentSlotUI.AddItem(playerInventoryManager.currentFeetEquipment);
        }
        else
        {
            feetEquipmentSlotUI.ClearItem();
        }
    }
}
