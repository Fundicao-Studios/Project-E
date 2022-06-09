using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerManager player;
    public ItemStatsWindowUI itemStatsWindowUI;
    public PlayerInventoryManager playerInventory;
    public EquipmentWindowUI equipmentWindowUI;
    QuickSlotsUI quickSlotsUI;
    
    [Header("Janelas do UI")]
    public GameObject hudWindow;
    public GameObject inventoryWindow;
    public GameObject equipmentScreenWindow;
    public GameObject weaponInventoryWindow;
    public GameObject headEquipmentInventoryWindow;
    public GameObject bodyEquipmentInventoryWindow;
    public GameObject legEquipmentInventoryWindow;
    public GameObject feetEquipmentInventoryWindow;
    public GameObject consumableInventoryWindow;
    public GameObject itemStatsWindow;

    [Header("Slot Da Janela De Equipamento Selecionado")]
    public bool rightHandSlot01Selected;
    public bool rightHandSlot02Selected;
    public bool leftHandSlot01Selected;
    public bool leftHandSlot02Selected;
    public bool consumableSlot01Selected;
    public bool consumableSlot02Selected;
    public bool headEquipmentSlotSelected;
    public bool bodyEquipmentSlotSelected;
    public bool legEquipmentSlotSelected;
    public bool feetEquipmentSlotSelected;

    [Header("Inventário de Armas")]
    public GameObject weaponInventorySlotPrefab;
    public Transform weaponInventorySlotsParent;
    WeaponInventorySlot[] weaponInventorySlots;

    [Header("Inventário De Máscaras")]
    public GameObject headEquipmentInventorySlotPrefab;
    public Transform headEquipmentInventorySlotParent;
    HeadEquipmentInventorySlot[] headEquipmentInventorySlots;

    [Header("Inventário De Peitorais")]
    public GameObject bodyEquipmentInventorySlotPrefab;
    public Transform bodyEquipmentInventorySlotParent;
    BodyEquipmentInventorySlot[] bodyEquipmentInventorySlots;

    [Header("Inventário De Calças")]
    public GameObject legEquipmentInventorySlotPrefab;
    public Transform legEquipmentInventorySlotParent;
    LegEquipmentInventorySlot[] legEquipmentInventorySlots;

    [Header("Inventário De Botas")]
    public GameObject feetEquipmentInventorySlotPrefab;
    public Transform feetEquipmentInventorySlotParent;
    FeetEquipmentInventorySlot[] feetEquipmentInventorySlots;

    [Header("Inventário De Consumíveis")]
    public GameObject consumableInventorySlotPrefab;
    public Transform consumableInventorySlotsParent;
    ConsumableInventorySlot[] consumableInventorySlots;

    private void Awake()
    {
        player = FindObjectOfType<PlayerManager>();
        playerInventory = FindObjectOfType<PlayerInventoryManager>();
        quickSlotsUI = GetComponentInChildren<QuickSlotsUI>();

        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        headEquipmentInventorySlots = headEquipmentInventorySlotParent.GetComponentsInChildren<HeadEquipmentInventorySlot>();
        bodyEquipmentInventorySlots = bodyEquipmentInventorySlotParent.GetComponentsInChildren<BodyEquipmentInventorySlot>();
        legEquipmentInventorySlots = legEquipmentInventorySlotParent.GetComponentsInChildren<LegEquipmentInventorySlot>();
        feetEquipmentInventorySlots = feetEquipmentInventorySlotParent.GetComponentsInChildren<FeetEquipmentInventorySlot>();
        consumableInventorySlots = consumableInventorySlotsParent.GetComponentsInChildren<ConsumableInventorySlot>();
    }

    private void Start()
    {
        equipmentWindowUI.LoadWeaponOnEquipmentScreen(playerInventory);

        equipmentWindowUI.LoadArmorOnEquipmentScreen(playerInventory);

        equipmentWindowUI.LoadConsumableOnEquipmentScreen(playerInventory);
        
        if (player.playerInventoryManager.currentSpell != null)
        {
            quickSlotsUI.UpdateCurrentSpellIcon(playerInventory.currentSpell);
        }
    }

    public void UpdateUI()
    {
        //SLOTS DO INVENTÁRIO DE ARMAS
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

        //SLOTS DO INVENTÁRIO DE CONSUMÍVEIS
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

        //SLOTS DO INVENTÁRIO DE MÁSCARAS
        for (int i = 0; i < headEquipmentInventorySlots.Length; i++)
        {
            if (i < player.playerInventoryManager.headEquipmentInventory.Count)
            {
                if (headEquipmentInventorySlots.Length < player.playerInventoryManager.headEquipmentInventory.Count)
                {
                    Instantiate(headEquipmentInventorySlotParent, headEquipmentInventorySlotParent);
                    headEquipmentInventorySlots = headEquipmentInventorySlotParent.GetComponentsInChildren<HeadEquipmentInventorySlot>();
                }

                headEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.headEquipmentInventory[i]);
            }
            else
            {
                headEquipmentInventorySlots[i].ClearInventorySlot();
            }
        }

        //SLOTS DO INVENTÁRIO DE PEITORAIS
        for (int i = 0; i < bodyEquipmentInventorySlots.Length; i++)
        {
            if (i < player.playerInventoryManager.bodyEquipmentInventory.Count)
            {
                if (bodyEquipmentInventorySlots.Length < player.playerInventoryManager.bodyEquipmentInventory.Count)
                {
                    Instantiate(bodyEquipmentInventorySlotParent, bodyEquipmentInventorySlotParent);
                    bodyEquipmentInventorySlots = bodyEquipmentInventorySlotParent.GetComponentsInChildren<BodyEquipmentInventorySlot>();
                }

                bodyEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.bodyEquipmentInventory[i]);
            }
            else
            {
                bodyEquipmentInventorySlots[i].ClearInventorySlot();
            }
        }

        //SLOTS DO INVENTÁRIO DE CALÇAS
        for (int i = 0; i < legEquipmentInventorySlots.Length; i++)
        {
            if (i < player.playerInventoryManager.legEquipmentInventory.Count)
            {
                if (legEquipmentInventorySlots.Length < player.playerInventoryManager.legEquipmentInventory.Count)
                {
                    Instantiate(legEquipmentInventorySlotParent, legEquipmentInventorySlotParent);
                    legEquipmentInventorySlots = legEquipmentInventorySlotParent.GetComponentsInChildren<LegEquipmentInventorySlot>();
                }

                legEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.legEquipmentInventory[i]);
            }
            else
            {
                legEquipmentInventorySlots[i].ClearInventorySlot();
            }
        }

        //SLOTS DO INVENTÁRIO DE BOTAS
        for (int i = 0; i < feetEquipmentInventorySlots.Length; i++)
        {
            if (i < player.playerInventoryManager.feetEquipmentInventory.Count)
            {
                if (feetEquipmentInventorySlots.Length < player.playerInventoryManager.feetEquipmentInventory.Count)
                {
                    Instantiate(feetEquipmentInventorySlotParent, feetEquipmentInventorySlotParent);
                    feetEquipmentInventorySlots = feetEquipmentInventorySlotParent.GetComponentsInChildren<FeetEquipmentInventorySlot>();
                }

                feetEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.feetEquipmentInventory[i]);
            }
            else
            {
                feetEquipmentInventorySlots[i].ClearInventorySlot();
            }
        }
    }

    public void OpenInventories()
    {
        inventoryWindow.SetActive(true);
        equipmentScreenWindow.SetActive(true);
    }

    public void CloseInventories()
    {
        inventoryWindow.SetActive(false);
        equipmentScreenWindow.SetActive(false);
    }

    public void CloseAllInventoryWindows()
    {
        ResetAllSelectedSlots();
        weaponInventoryWindow.SetActive(false);
        headEquipmentInventoryWindow.SetActive(false);
        bodyEquipmentInventoryWindow.SetActive(false);
        legEquipmentInventoryWindow.SetActive(false);
        feetEquipmentInventoryWindow.SetActive(false);
        consumableInventoryWindow.SetActive(false);
        equipmentScreenWindow.SetActive(false);
        itemStatsWindow.SetActive(false);
    }

    public void ResetAllSelectedSlots()
    {
        rightHandSlot01Selected = false;
        rightHandSlot02Selected = false;
        leftHandSlot01Selected = false;
        leftHandSlot02Selected = false;

        consumableSlot01Selected = false;
        consumableSlot02Selected = false;

        headEquipmentSlotSelected = false;
        bodyEquipmentSlotSelected = false;
        legEquipmentSlotSelected = false;
        feetEquipmentSlotSelected = false;
    }
}
