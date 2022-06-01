using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocWeaponSlotManager : CharacterWeaponSlotsManager
{
    public WeaponItem mouthWeapon;
    public GameObject spellCastFX;
    
    public WeaponHolderSlot mouthWeaponSlot;

    DamageCollider mouthDamageCollider;

    CrocStatsManager crocStatsManager;

    private void Awake()
    {
        crocStatsManager = GetComponentInParent<CrocStatsManager>();
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
            mouthWeaponSlot = weaponSlot;
        }
    }

    public void LoadWeaponOnSlot(WeaponItem weapon)
    {
        mouthWeaponSlot.currentWeapon = weapon;
        mouthWeaponSlot.LoadWeaponModel(weapon);
        LoadWeaponsDamageCollider();
    }

    public void LoadWeaponsOnBothHands()
    {
        LoadWeaponOnSlot(mouthWeapon);
    }

    public void LoadWeaponsDamageCollider()
    {
        mouthDamageCollider = mouthWeaponSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        mouthDamageCollider.characterManager = GetComponentInParent<CharacterManager>();

        mouthDamageCollider.physicalDamage = mouthWeapon.physicalDamage;
        mouthDamageCollider.fireDamage = mouthWeapon.fireDamage;

        mouthDamageCollider.teamIDNumber = crocStatsManager.teamIDNumber;
    }

    public void OpenDamageCollider()
    {
        mouthDamageCollider.EnableDamageCollider();
    }

    public void CloseDamageCollider()
    {
        mouthDamageCollider.DisableDamageCollider();
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
        //anim.SetBool("canCombo", true);
    }

    #region Controlar O Bonus De Poise Da Arma

    public void GrantWeaponAttackingPoiseBonus()
    {
        crocStatsManager.totalPoiseDefense = crocStatsManager.totalPoiseDefense + crocStatsManager.offensivePoiseBonus;
    }

    public void ResetWeaponAttackingPoiseBonus()
    {
        crocStatsManager.totalPoiseDefense = crocStatsManager.armorPoiseBonus;
    }

    #endregion
}
