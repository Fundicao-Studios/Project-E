using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSlotManager : CharacterWeaponSlotsManager
{
    QuickSlotsUI quickSlotsUI;
    InputManager inputManager;
    Animator animator;
    PlayerManager playerManager;
    PlayerInventoryManager playerInventoryManager;
    PlayerStatsManager playerStatsManager;
    PlayerEffectsManager playerEffectsManager;
    CameraManager cameraManager;

    [Header("Arma De Ataque")]
    public WeaponItem attackingWeapon;

    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        inputManager = GetComponentInParent<InputManager>();
        playerStatsManager = GetComponentInParent<PlayerStatsManager>();
        playerEffectsManager = GetComponentInParent<PlayerEffectsManager>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerInventoryManager = GetComponentInParent<PlayerInventoryManager>();
        animator = GetComponent<Animator>();
        quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
        LoadWeaponHolderSlots();
    }

    private void LoadWeaponHolderSlots()
    {
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
        LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
        LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (weaponItem != null)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                animator.CrossFade(weaponItem.left_hand_idle, 0.2f);
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
                    animator.CrossFade("Both Arms Empty", 0.2f);
                    backSlot.UnloadWeaponAndDestroy();
                    animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
                }

                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
            }
        }
        else
        {
            weaponItem = unarmedWeapon;

            if (isLeft)
            {
                animator.CrossFade("Left Arm Empty", 0.2f);
                playerInventoryManager.leftWeapon = unarmedWeapon;
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
            }
            else
            {
                animator.CrossFade("Right Arm Empty", 0.2f);
                playerInventoryManager.rightWeapon = unarmedWeapon;
                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
            }
        }
    }

    public void SucessfullyThrowBomb()
    {
        Destroy(playerEffectsManager.instantiadedFXModel);
        BombConsumableItem bombItem = playerInventoryManager.currentConsumable as BombConsumableItem;

        GameObject activeModelBomb = Instantiate(bombItem.liveBombModel, rightHandSlot.transform.position, cameraManager.cameraPivotTransform.rotation);
        activeModelBomb.transform.rotation = Quaternion.Euler(cameraManager.cameraPivotTransform.eulerAngles.x, playerManager.lockOnTransform.eulerAngles.y, 0);
        BombDamageCollider damageCollider = activeModelBomb.GetComponentInChildren<BombDamageCollider>();
        
        damageCollider.explosionDamage = bombItem.baseDamage;
        damageCollider.explosionSplashDamage = bombItem.explosiveDamage;
        damageCollider.bombRigidBody.AddForce(activeModelBomb.transform.forward * bombItem.forwardVelocity);
        damageCollider.bombRigidBody.AddForce(activeModelBomb.transform.up * bombItem.upwardVelocity);
        damageCollider.teamIDNumber = playerStatsManager.teamIDNumber;
        LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
    }

    #region Controlar os Damage Colliders das armas

    private void LoadLeftWeaponDamageCollider()
    {
        leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

        leftHandDamageCollider.physicalDamage = playerInventoryManager.leftWeapon.physicalDamage;
        leftHandDamageCollider.fireDamage = playerInventoryManager.leftWeapon.fireDamage;

        leftHandDamageCollider.teamIDNumber = playerStatsManager.teamIDNumber;

        leftHandDamageCollider.poiseBreak = playerInventoryManager.leftWeapon.poiseBreak;
    }

    private void LoadRightWeaponDamageCollider()
    {
        rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

        rightHandDamageCollider.physicalDamage = playerInventoryManager.rightWeapon.physicalDamage;
        rightHandDamageCollider.fireDamage = playerInventoryManager.rightWeapon.fireDamage;

        rightHandDamageCollider.teamIDNumber = playerStatsManager.teamIDNumber;

        rightHandDamageCollider.poiseBreak = playerInventoryManager.rightWeapon.poiseBreak;
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
        playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
    }

    public void DrainStaminaHeavyAttack()
    {
        playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
    }
    #endregion

    #region Controlar O Bonus De Poise Da Arma

    public void GrantWeaponAttackingPoiseBonus()
    {
        playerStatsManager.totalPoiseDefense = playerStatsManager.totalPoiseDefense + attackingWeapon.offensivePoiseBonus;
    }

    public void ResetWeaponAttackingPoiseBonus()
    {
        playerStatsManager.totalPoiseDefense = playerStatsManager.armorPoiseBonus;
    }

    #endregion
}
