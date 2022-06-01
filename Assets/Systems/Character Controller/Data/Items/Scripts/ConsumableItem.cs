using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
{
    public GameObject itemModel;

    [Header("Quantide De Items")]
    public int maxItemAmount;
    public int currentItemAmount;

    [Header("Animações")]
    public string consumeAnimation;
    public string hand_idle;
    public bool isInteracting;

    public virtual void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, ConsumableSlotManager consumableSlotManager, PlayerEffectsManager playerEffectsManager)
    {
        if (currentItemAmount > 0)
        {
            playerAnimatorManager.PlayTargetAnimation(consumeAnimation, isInteracting, true);
        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation("Shrug", isInteracting, true);
        }
    }
}
