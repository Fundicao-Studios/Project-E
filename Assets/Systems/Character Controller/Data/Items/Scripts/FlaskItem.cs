using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumables/Potion")]
public class FlaskItem : ConsumableItem
{
    [Header("Tipo De Poção")]
    public bool lifePotion;
    public bool manaPotion;
    public bool emptyPotion;
    public bool isEmpty;

    [Header("Quantidade De Recuperação")]
    public int healthRecoverAmount;
    public int manaPointsRecoverAmount;

    [Header("VFX De Recuperação")]
    public GameObject recoveryFX;

    public FlaskItem typeOfPotion;

    private void Awake()
    {
        typeOfPotion = FindObjectOfType<FlaskItem>();
    }

    public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, ConsumableSlotManager consumableSlotManager, PlayerEffectsManager playerEffectsManager)
    {
        base.AttemptToConsumeItem(playerAnimatorManager, consumableSlotManager, playerEffectsManager);
        GameObject potion = Instantiate(itemModel, consumableSlotManager.consumableHandSlot.transform);
        playerEffectsManager.currentParticleFX = recoveryFX;
        playerEffectsManager.amountToBeHealed = healthRecoverAmount;
        playerEffectsManager.instantiadedFXModel = potion;
        consumableSlotManager.consumableHandSlot.UnloadConsumable();
    }
}
