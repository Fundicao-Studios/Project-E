using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemWeaponSlotManager : CharacterWeaponSlotsManager
{
    public WeaponItem rightHandWeapon;
    public WeaponItem leftHandWeapon;

    GolemStatsManager golemStatsManager;

    private void Awake()
    {
        golemStatsManager = GetComponentInParent<GolemStatsManager>();
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

            leftHandDamageCollider.physicalDamage = leftHandWeapon.physicalDamage;
            leftHandDamageCollider.fireDamage = leftHandWeapon.fireDamage;

            leftHandDamageCollider.teamIDNumber = golemStatsManager.teamIDNumber;
        }
        else
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();

            rightHandDamageCollider.physicalDamage = rightHandWeapon.physicalDamage;
            rightHandDamageCollider.fireDamage = rightHandWeapon.fireDamage;

            rightHandDamageCollider.teamIDNumber = golemStatsManager.teamIDNumber;
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
        golemStatsManager.totalPoiseDefense = golemStatsManager.totalPoiseDefense + golemStatsManager.offensivePoiseBonus;
    }

    public void ResetWeaponAttackingPoiseBonus()
    {
        golemStatsManager.totalPoiseDefense = golemStatsManager.armorPoiseBonus;
    }

    #endregion
}
