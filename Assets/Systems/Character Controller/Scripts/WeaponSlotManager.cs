using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerInventory playerInventory;

    public WeaponItem attackingWeapon;

    public WeaponHolderSlot leftHandSlot;
    public WeaponHolderSlot rightHandSlot;
    WeaponHolderSlot backSlot;

    public DamageCollider leftHandDamageCollider;
    public DamageCollider rightHandDamageCollider;

    Animator animator;

    QuickSlotsUI quickSlotsUI;

    PlayerStats playerStats;
    InputManager inputManager;

    private void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        playerInventory = GetComponentInParent<PlayerInventory>();
        animator = GetComponent<Animator>();
        quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
        playerStats = GetComponentInParent<PlayerStats>();
        inputManager = GetComponentInParent<InputManager>();

        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.isLeftHandSlot)
            {
                leftHandSlot = weaponSlot;
            }
            else if (weaponSlot.isRightHandSlot)
            {
                rightHandSlot = weaponSlot;
            }
            else if (weaponSlot.isBackSlot)
            {
                backSlot = weaponSlot;
            }
        }
    }

    public void LoadBothWeaponsOnSlots()
    {
        LoadWeaponOnSlot(playerInventory.rightWeapon, false);
        LoadWeaponOnSlot(playerInventory.leftWeapon, true);
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (isLeft)
        {
            leftHandSlot.currentWeapon = weaponItem;
            leftHandSlot.LoadWeaponModel(weaponItem);
            LoadLeftWeaponDamageCollider();
            quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
            #region Controlar As Animações De Idle Da Arma Da Mão Esquerda
            if (weaponItem != null)
            {
                animator.CrossFade(weaponItem.left_hand_idle, 0.2f);
            }
            else
            {
                animator.CrossFade("Left Arm Empty", 0.2f);
            }
            #endregion
        }
        else
        {
            if (inputManager.twoHandFlag)
            {
                backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                leftHandSlot.UnloadWeaponAndDestroy();
                animator.CrossFade(weaponItem.th_idle, 0.2f);
            }
            else
            {
                #region Controlar As Animações De Idle Da Arma Da Mão Direita

                animator.CrossFade("Both Arms Empty", 0.2f);

                backSlot.UnloadWeaponAndDestroy();
                
                if (weaponItem != null)
                {
                    animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Right Arm Empty", 0.2f);
                }
                #endregion
            }

            rightHandSlot.currentWeapon = weaponItem;
            rightHandSlot.LoadWeaponModel(weaponItem);
            LoadRightWeaponDamageCollider();
            quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
        }
    }

    #region Controlar os Damage Colliders das armas

    private void LoadLeftWeaponDamageCollider()
    {
        leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        leftHandDamageCollider.currentWeaponDamage = playerInventory.leftWeapon.baseDamage;
        leftHandDamageCollider.poiseBreak = playerInventory.leftWeapon.poiseBreak;
    }

    private void LoadRightWeaponDamageCollider()
    {
        rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        rightHandDamageCollider.currentWeaponDamage = playerInventory.rightWeapon.baseDamage;
        rightHandDamageCollider.poiseBreak = playerInventory.rightWeapon.poiseBreak;
    }

    public void OpenDamageCollider()
    {
        if (playerManager.isUsingRightHand)
        {
            rightHandDamageCollider.EnableDamageCollider();
        }
        else if (playerManager.isUsingLeftHand)
        {
            leftHandDamageCollider.EnableDamageCollider();
        }
    }

    public void CloseDamageCollider()
    {
        if (rightHandDamageCollider != null)
        {
            rightHandDamageCollider.DisableDamageCollider();
        }

        if (leftHandDamageCollider != null)
        {
            leftHandDamageCollider.DisableDamageCollider();
        }
    }

    #endregion

    #region Controlar o gasto de Stamina
    public void DrainStaminaLightAttack()
    {
        playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
    }

    public void DrainStaminaHeavyAttack()
    {
        playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
    }
    #endregion

    #region Controlar O Bonus De Poise Da Arma

    public void GrantWeaponAttackingPoiseBonus()
    {
        playerStats.totalPoiseDefense = playerStats.totalPoiseDefense + attackingWeapon.offensivePoiseBonus;
    }

    public void ResetWeaponAttackingPoiseBonus()
    {
        playerStats.totalPoiseDefense = playerStats.armorPoiseBonus;
    }

    #endregion
}
