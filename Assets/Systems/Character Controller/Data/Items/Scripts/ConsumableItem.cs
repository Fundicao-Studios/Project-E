using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
{
    [Header("Quantide De Items")]
    public int maxItemAmount;

    [Header("Modelo Do Item")]
    public GameObject itemModel;

    [Header("Animações")]
    public string consumableAnimation;
    public bool isInteracting;

    public FlaskItem typeOfPotion;

    private void Awake()
    {
        typeOfPotion = FindObjectOfType<FlaskItem>();
    }

    public virtual void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, WeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
    {
        if (maxItemAmount > 0)
        {
            playerAnimatorManager.PlayTargetAnimation(consumableAnimation, isInteracting, true);
        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation("Shrug", isInteracting, true);
        }
    }
}
