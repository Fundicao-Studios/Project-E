using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableSlotManager : MonoBehaviour
{
    PlayerManager playerManager;
    Animator animator;
    PlayerInventoryManager playerInventoryManager;

    public ConsumableItem emptyBottle;

    public ConsumableHolderSlot consumableHandSlot;

    QuickSlotsUI quickSlotsUI;

    PlayerStatsManager playerStatsManager;
    InputManager inputManager;

    private void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        playerInventoryManager = GetComponentInParent<PlayerInventoryManager>();
        quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
        playerStatsManager = GetComponentInParent<PlayerStatsManager>();
        animator = GetComponent<Animator>();
        inputManager = GetComponentInParent<InputManager>();
    }

    public void LoadConsumableOnSlot(ConsumableItem consumableItem)
    {
        if (consumableItem != null)
        {
            consumableHandSlot.currentConsumable = consumableItem;
            consumableHandSlot.LoadConsumableModel(consumableItem);
            quickSlotsUI.UpdateCurrentConsumableIcon(consumableItem);
            animator.CrossFade(consumableItem.hand_idle, 0.2f);  
        }
        else
        {
            consumableItem = emptyBottle;

            animator.CrossFade("Both Arms Empty", 0.2f);
            playerInventoryManager.currentConsumable = emptyBottle;
            consumableHandSlot.currentConsumable = consumableItem;
            consumableHandSlot.LoadConsumableModel(consumableItem);
            quickSlotsUI.UpdateCurrentConsumableIcon(consumableItem);
        }
    }
}
