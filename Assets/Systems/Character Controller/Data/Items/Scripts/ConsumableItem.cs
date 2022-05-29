using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
{
    public GameObject modelPrefab;
    public bool isEmpty;

    [Header("Quantide De Items")]
    public int maxItemAmount;

    [Header("Animações")]
    public string consumableAnimation;
    public string hand_idle;
    public bool isInteracting;

    public FlaskItem typeOfPotion;

    private void Awake()
    {
        typeOfPotion = FindObjectOfType<FlaskItem>();
    }

    public virtual void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
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
