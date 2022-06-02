using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    PlayerWeaponSlotManager playerWeaponSlotManager;
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

    [Header("Inventário De Equipamento")]
    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];
    public ConsumableItem[] consumablesInSlots = new ConsumableItem[1];

    public int currentRightWeaponIndex = 0;
    public int currentLeftWeaponIndex = 0;
    public int currentConsumableIndex = 0;

    [Header("Inventários")]
    public List<WeaponItem> weaponsInventory;
    public List<ConsumableItem> consumablesInventory;

    private void Awake()
    {
        playerWeaponSlotManager = GetComponentInChildren<PlayerWeaponSlotManager>();
        consumableSlotManager = GetComponentInChildren<ConsumableSlotManager>();
    }

    private void Start()
    {
        //rightWeapon = weaponsInRightHandSlots[0];
        //leftWeapon = weaponsInLeftHandSlots[0];
        //currentConsumable = consumablesInSlots[0];
        playerWeaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        playerWeaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        consumableSlotManager.LoadConsumableOnSlot(currentConsumable);
    }

    public void ChangeRightWeapon()
    {
        currentRightWeaponIndex = currentRightWeaponIndex + 1;

        if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] != null)
        {
            rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            playerWeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);

            if (rightWeapon.weaponType == WeaponType.ShockCaster)
            {
                currentSpell = shockSpell;
            }
            else if (rightWeapon.weaponType == WeaponType.WaterCaster)
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
            playerWeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);

            if (rightWeapon.weaponType == WeaponType.ShockCaster)
            {
                currentSpell = shockSpell;
            }
            else if (rightWeapon.weaponType == WeaponType.WaterCaster)
            {
                currentSpell = healSpell;
            }
        }
        else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] == null)
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;
        }

        if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
        {
            currentRightWeaponIndex = -1;
            rightWeapon = playerWeaponSlotManager.unarmedWeapon;
            playerWeaponSlotManager.LoadWeaponOnSlot(playerWeaponSlotManager.unarmedWeapon, false);
        }
    }

    public void ChangeLeftWeapon()
    {
        currentLeftWeaponIndex = currentLeftWeaponIndex + 1;

        if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] != null)
        {
            leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            playerWeaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);

            if (leftWeapon.weaponType == WeaponType.ShockCaster)
            {
                currentSpell = shockSpell;
            }
            else if (leftWeapon.weaponType == WeaponType.WaterCaster)
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
            playerWeaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);

            if (leftWeapon.weaponType == WeaponType.ShockCaster)
            {
                currentSpell = shockSpell;
            }
            else if (leftWeapon.weaponType == WeaponType.WaterCaster)
            {
                currentSpell = healSpell;
            }
        }
        else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] == null)
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
        }

        if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
        {
            currentLeftWeaponIndex = -1;
            rightWeapon = playerWeaponSlotManager.unarmedWeapon;
            playerWeaponSlotManager.LoadWeaponOnSlot(playerWeaponSlotManager.unarmedWeapon, true);
        }
    }

    public void ChangeConsumable()
    {
        currentConsumableIndex = currentConsumableIndex + 1;

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

        if (currentConsumableIndex > consumablesInSlots.Length - 1)
        {
            currentConsumableIndex = -1;
            currentConsumable = consumableSlotManager.emptyBottle;
            consumableSlotManager.LoadConsumableOnSlot(consumableSlotManager.emptyBottle);
        }
    }
}
