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

    [Header("Quantidade De Recuperação")]
    public int healthRecoverAmount;
    public int manaPointsRecoverAmount;

    [Header("VFX De Recuperação")]
    public GameObject recoveryFX;

    public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
    {
        base.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
        GameObject potion = Instantiate(modelPrefab, weaponSlotManager.rightHandSlot.transform);
        playerEffectsManager.currentParticleFX = recoveryFX;
        playerEffectsManager.amountToBeHealed = healthRecoverAmount;
        playerEffectsManager.instantiadedFXModel = potion;
        weaponSlotManager.rightHandSlot.UnloadWeapon();
    }
}
