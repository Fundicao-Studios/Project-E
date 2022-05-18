using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    WeaponSlotManager weaponSlotManager;
    ConsumableSlotManager consumableSlotManager;

    [Header("Tipos De Magias")]
    public SpellItem shockSpell;
    public SpellItem healSpell;

    [Header("Equipamento A Ser Usado")]
    public SpellItem currentSpell;
    public WeaponItem rightWeapon;
    public WeaponItem leftWeapon;
    public ConsumableItem currentConsumable;

    [Header("Armadura Equipada")]
    public HelmetEquipment currentHelmetEquipment;
    public TorsoEquipment currentTorsoEquipment;
    public FeetEquipment currentFeetEquipment;
    public LegEquipment currentLegEquipment;

    public WeaponItem unarmedWeapon;

    [Header("Inventário De Equipamento")]
    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];
    public ConsumableItem[] consumablesInSlots = new ConsumableItem[1];

    public int currentRightWeaponIndex = -1;
    public int currentLeftWeaponIndex = -1;
    public int currentConsumableIndex = -1;

    [Header("Inventários")]
    public List<WeaponItem> weaponsInventory;
    public List<ConsumableItem> consumablesInventory;

    private void Awake()
    {
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        consumableSlotManager = GetComponentInChildren<ConsumableSlotManager>();
    }

    private void Start()
    {
        rightWeapon = weaponsInRightHandSlots[0];
        leftWeapon = weaponsInLeftHandSlots[0];
        currentConsumable = consumablesInSlots[0];
        weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
    }

    public void ChangeRightWeapon()
    {
        currentRightWeaponIndex = currentRightWeaponIndex + 1;

        if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
        {
            currentRightWeaponIndex = 0;
            rightWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
        }

        if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] != null)
        {
            rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);

            if (rightWeapon.isShockCaster)
            {
                currentSpell = shockSpell;
            }
            else if (rightWeapon.isWaterCaster)
            {
                currentSpell = healSpell;
            }
        }
        else if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;
        }

        else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] != null)
        {
            rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);

            if (rightWeapon.isShockCaster)
            {
                currentSpell = shockSpell;
            }
            else if (rightWeapon.isWaterCaster)
            {
                currentSpell = healSpell;
            }
        }
        else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] == null)
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;
        }
    }

    public void ChangeLeftWeapon()
    {
        currentLeftWeaponIndex = currentLeftWeaponIndex + 1;

        if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
        {
            currentLeftWeaponIndex = 0;
            leftWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true);
        }

        if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] != null)
        {
            leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);

            if (leftWeapon.isShockCaster)
            {
                currentSpell = shockSpell;
            }
            else if (leftWeapon.isWaterCaster)
            {
                currentSpell = healSpell;
            }
        }
        else if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] == null)
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
        }

        else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] != null)
        {
            leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);

            if (leftWeapon.isShockCaster)
            {
                currentSpell = shockSpell;
            }
            else if (leftWeapon.isWaterCaster)
            {
                currentSpell = healSpell;
            }
        }
        else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] == null)
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
        }
    }

    public void ChangeConsumable()
    {
        currentConsumableIndex = currentConsumableIndex + 1;

        if (currentConsumableIndex > consumablesInSlots.Length - 1)
        {
            currentConsumableIndex = 0;
            currentConsumable = consumablesInSlots[0];
            consumableSlotManager.LoadConsumableOnSlot(consumablesInSlots[0]);
        }

        if (currentConsumableIndex == 0 && consumablesInSlots[0] != null)
        {
            currentConsumable = consumablesInSlots[currentConsumableIndex];
            consumableSlotManager.LoadConsumableOnSlot(consumablesInSlots[currentConsumableIndex]);
        }
        else if (currentConsumableIndex == 0 && consumablesInSlots[0] == null)
        {
            currentConsumableIndex = currentConsumableIndex + 1;
        }

        else if (currentConsumableIndex == 1 && consumablesInSlots[1] != null)
        {
            currentConsumable = consumablesInSlots[currentConsumableIndex];
            consumableSlotManager.LoadConsumableOnSlot(consumablesInSlots[currentConsumableIndex]);
        }
        else if (currentConsumableIndex == 1 && consumablesInSlots[1] == null)
        {
            currentConsumableIndex = currentConsumableIndex + 1;
        }
    }
}
