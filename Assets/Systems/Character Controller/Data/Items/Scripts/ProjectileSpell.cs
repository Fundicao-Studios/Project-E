using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Projectile Spell")]
public class ProjectileSpell : SpellItem
{
    [Header("Dano Do Projetil")]
    public float baseDamage;

    [Header("Físicas Do Projetil")]
    public float projectileForwardVelocity;
    public float projectileUpwardVelocity;
    public float projectileMass;
    public bool isEffectedByGravity;
    Rigidbody rigidBody;

    public override void AttemptToCastSpell(
        PlayerAnimatorManager animatorManager, 
        PlayerStatsManager playerStats, 
        PlayerWeaponSlotManager weaponSlotManager)
    {
        base.AttemptToCastSpell(animatorManager, playerStats, weaponSlotManager);
        GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, weaponSlotManager.rightHandSlot.transform);
        animatorManager.PlayTargetAnimation(spellAnimation, true);
    }

    public override void SuccessfullyCastSpell(
        PlayerAnimatorManager animatorManager, 
        PlayerStatsManager playerStats,
        CameraManager cameraManager,
        PlayerWeaponSlotManager weaponSlotManager)
    {
        base.SuccessfullyCastSpell(animatorManager, playerStats, cameraManager, weaponSlotManager);
        GameObject instantiatedSpellFX = Instantiate(spellCastFX, weaponSlotManager.rightHandSlot.transform.position, cameraManager.cameraPivotTransform.rotation);
        rigidBody = instantiatedSpellFX.GetComponent<Rigidbody>();
        //spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();

        if (cameraManager.currentLockOnTarget != null)
        {
            instantiatedSpellFX.transform.LookAt(cameraManager.currentLockOnTarget.transform);
        }
        else
        {
            instantiatedSpellFX.transform.rotation = Quaternion.Euler(cameraManager.cameraPivotTransform.eulerAngles.x, playerStats.transform.eulerAngles.y, 0);
        }

        rigidBody.AddForce(instantiatedSpellFX.transform.forward * projectileForwardVelocity);
        rigidBody.AddForce(instantiatedSpellFX.transform.up * projectileUpwardVelocity);
        rigidBody.useGravity = isEffectedByGravity;
        rigidBody.mass = projectileMass;
        instantiatedSpellFX.transform.parent = null;
    }
}
