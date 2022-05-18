using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public EquipmentWindowUI equipmentWindowUI;
    
    [Header("Janelas do UI")]
    public GameObject hudWindow;
    public GameObject selectWindow;
    public GameObject equipmentScreenWindow;
    public GameObject weaponInventoryWindow;
    public GameObject consumableInventoryWindow;

    [Header("Slot Da Janela De Equipamento Selecionado")]
    public bool rightHandSlot01Selected;
    public bool rightHandSlot02Selected;
    public bool leftHandSlot01Selected;
    public bool leftHandSlot02Selected;
    public bool consumableSlot01Selected;
    public bool consumableSlot02Selected;

    [Header("Inventário de Armas")]
    public GameObject weaponInventorySlotPrefab;
    public Transform weaponInventorySlotsParent;
    WeaponInventorySlot[] weaponInventorySlots;

    [Header("Inventário De Consumíveis")]
    public GameObject consumableInventorySlotPrefab;
    public Transform consumableInventorySlotsParent;
    ConsumableInventorySlot[] consumableInventorySlots;

    private void Awake()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
    }

    private void Start()
    {
        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        consumableInventorySlots = consumableInventorySlotsParent.GetComponentsInChildren<ConsumableInventorySlot>();
        equipmentWindowUI.LoadWeaponOnEquipmentScreen(playerInventory);
    }

    public void UpdateUI()
    {
        #region Slots do Inventário de Armas
        for (int i = 0; i < weaponInventorySlots.Length; i++)
        {
            if (i < playerInventory.weaponsInventory.Count)
            {
                if (weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                {
                    Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                    weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                }
                weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
            }
            else
            {
                weaponInventorySlots[i].ClearInventorySlot();
            }
        }
        #endregion

        #region Slots Do Inventário De Consumíveis
        for (int i = 0; i < consumableInventorySlots.Length; i++)
        {
            if (i < playerInventory.consumablesInventory.Count)
            {
                if (consumableInventorySlots.Length < playerInventory.consumablesInventory.Count)
                {
                    Instantiate(consumableInventorySlotPrefab, consumableInventorySlotsParent);
                    consumableInventorySlots = consumableInventorySlotsParent.GetComponentsInChildren<ConsumableInventorySlot>();
                }
                consumableInventorySlots[i].AddItem(playerInventory.consumablesInventory[i]);
            }
            else
            {
                consumableInventorySlots[i].ClearInventorySlot();
            }
        }
        #endregion
    }

    public void OpenSelectWindow()
    {
        selectWindow.SetActive(true);
    }

    public void CloseSelectWindow()
    {
        selectWindow.SetActive(false);
    }

    public void CloseAllInventoryWindows()
    {
        ResetAllSelectedSlots();
        weaponInventoryWindow.SetActive(false);
        consumableInventoryWindow.SetActive(false);
        equipmentScreenWindow.SetActive(false);
    }

    public void ResetAllSelectedSlots()
    {
        rightHandSlot01Selected = false;
        rightHandSlot02Selected = false;
        leftHandSlot01Selected = false;
        leftHandSlot02Selected = false;
        consumableSlot01Selected = false;
        consumableSlot02Selected = false;
    }
}
