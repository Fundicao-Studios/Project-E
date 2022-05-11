using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerInventory playerInventory;
    public BlockingCollider blockingCollider;

    private void Awake()
    {
        inputManager = GetComponentInParent<InputManager>();
        playerInventory = GetComponentInParent<PlayerInventory>();
    }

    public void OpenBlockingCollider()
    {
        if (inputManager.twoHandFlag)
        {
            blockingCollider.SetColliderDamageAbsorption(playerInventory.rightWeapon);
        }
        else
        {
            blockingCollider.SetColliderDamageAbsorption(playerInventory.leftWeapon);
        }

        blockingCollider.EnableBlockingCollider();
    }

    public void CloseBlockingCollider()
    {
        blockingCollider.DisableBlockingCollider();
    }
}
