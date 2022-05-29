using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeaponSlotManager : CharacterWeaponSlotsManager
{
    public WeaponItem rightHandWeapon;
    public WeaponItem leftHandWeapon;

    BossStatsManager bossStatsManager;

    private void Awake()
    {
        bossStatsManager = GetComponentInParent<BossStatsManager>();
        LoadWeaponHolderSlots();
    }

    private void Start()
    {
        LoadWeaponsOnBothHands();
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
        }
    }

    public void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
    {
        if (isLeft)
        {
            leftHandSlot.currentWeapon = weapon;
            leftHandSlot.LoadWeaponModel(weapon);
            LoadWeaponsDamageCollider(true);
        }
        else
        {
            rightHandSlot.currentWeapon = weapon;
            rightHandSlot.LoadWeaponModel(weapon);
            LoadWeaponsDamageCollider(false);
        }
    }

    public void LoadWeaponsOnBothHands()
    {
        if (rightHandWeapon != null)
        {
            LoadWeaponOnSlot(rightHandWeapon, false);
        }
        if (leftHandWeapon != null)
        {
            LoadWeaponOnSlot(leftHandWeapon, true);
        }
    }

    public void LoadWeaponsDamageCollider(bool isLeft)
    {
        if (isLeft)
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
        }
        else
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
        }
    }

    public void OpenRightDamageCollider()
    {
        rightHandDamageCollider.EnableDamageCollider();
    }

    public void CloseRightDamageCollider()
    {
        rightHandDamageCollider.DisableDamageCollider();
    }

    public void OpenLeftDamageCollider()
    {
        leftHandDamageCollider.EnableDamageCollider();
    }

    public void CloseLeftDamageCollider()
    {
        leftHandDamageCollider.DisableDamageCollider();
    }

    public void DrainStaminaLightAttack()
    {
        
    }

    public void DrainStaminaHeavyAttack()
    {
        
    }

    public void EnableCombo()
    {
        //anim.SetBool("canCombo", true);
    }

    public void DisableCombo()
    {
        //anim.SetBool("canCombo", false);
    }

    #region Controlar O Bonus De Poise Da Arma

    public void GrantWeaponAttackingPoiseBonus()
    {
        bossStatsManager.totalPoiseDefense = bossStatsManager.totalPoiseDefense + bossStatsManager.offensivePoiseBonus;
    }

    public void ResetWeaponAttackingPoiseBonus()
    {
        bossStatsManager.totalPoiseDefense = bossStatsManager.armorPoiseBonus;
    }

    #endregion
}
