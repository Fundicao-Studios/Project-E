using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableSlotManager : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerInventory playerInventory;

    QuickSlotsUI quickSlotsUI;

    PlayerStats playerStats;
    InputManager inputManager;

    private void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        playerInventory = GetComponentInParent<PlayerInventory>();
        quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
        playerStats = GetComponentInParent<PlayerStats>();
        inputManager = GetComponentInParent<InputManager>();
    }

    public void LoadConsumableOnSlot(ConsumableItem consumableItem)
    {
        playerInventory.currentConsumable = consumableItem;
        quickSlotsUI.UpdateConsumableQuickSlotsUI(consumableItem);
    }
}
