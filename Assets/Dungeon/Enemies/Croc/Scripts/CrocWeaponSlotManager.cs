using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocWeaponSlotManager : MonoBehaviour
{
    public WeaponItem mouthWeapon;
    public GameObject spellCastFX;
    
    public WeaponHolderSlot mouthWeaponSlot;

    DamageCollider mouthDamageCollider;

    private void Awake()
    {
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            mouthWeaponSlot = weaponSlot;
        }
    }

    private void Start()
    {
        LoadWeaponsOnBothHands();
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
        
    }

    public void DisableCombo()
    {
        
    }
}
