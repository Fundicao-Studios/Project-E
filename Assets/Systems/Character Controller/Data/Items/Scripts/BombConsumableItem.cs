using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumables/Bomb Item")]
public class BombConsumableItem : ConsumableItem
{
    [Header("Velocidade")]
    public int upwardVelocity = 50;
    public int forwardVelocity = 50;
    public int bombMass = 1;

    [Header("Modelo Atual Da Bomba")]
    public GameObject liveBombModel;

    [Header("Dano Base")]
    public int baseDamage = 200;
    public int explosiveDamage = 75;

    public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, ConsumableSlotManager consumableSlotManager, PlayerEffectsManager playerEffectsManager)
    {
        if (currentItemAmount > 0)
        {
            consumableSlotManager.consumableHandSlot.UnloadConsumable();
            playerAnimatorManager.PlayTargetAnimation(consumeAnimation, true);
            GameObject bombModel = Instantiate(itemModel, consumableSlotManager.consumableHandSlot.transform.position, Quaternion.identity, consumableSlotManager.consumableHandSlot.transform);
            playerEffectsManager.instantiadedFXModel = bombModel;
        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation("Shrug", true);
        }
    }
}
