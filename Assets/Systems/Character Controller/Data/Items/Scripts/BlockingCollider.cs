using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingCollider : MonoBehaviour
{
    public BoxCollider blockingCollider;

    public float blockingPhysicalDamageAbsorption;
    public float blockingFireDamageAbsorption;
    public float blockingShockDamageAbsorption;

    private void Awake()
    {
        blockingCollider = GetComponent<BoxCollider>();
    }

    public void SetColliderDamageAbsorption(WeaponItem weapon)
    {
        if (weapon != null)
        {
            blockingPhysicalDamageAbsorption = weapon.physicalDamageAbsorption;
            blockingFireDamageAbsorption = weapon.fireDamageAbsorption;
            blockingShockDamageAbsorption = weapon.shockDamageAbsorption;
        }
    }

    public void EnableBlockingCollider()
    {
        blockingCollider.enabled = true;
    }

    public void DisableBlockingCollider()
    {
        blockingCollider.enabled = false;
    }
}
