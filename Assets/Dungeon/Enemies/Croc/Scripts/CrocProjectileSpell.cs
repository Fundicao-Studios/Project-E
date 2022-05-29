using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Croc Projectile Spell")]
public class CrocProjectileSpell : CrocSpellItem
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
        CrocAnimatorManager animatorManager, 
        CrocStatsManager playerStats, 
        CrocWeaponSlotManager weaponSlotManager)
    {
        base.AttemptToCastSpell(animatorManager, playerStats, weaponSlotManager);
        GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, weaponSlotManager.mouthWeaponSlot.transform.position, weaponSlotManager.mouthWeaponSlot.transform.rotation);
        instantiatedWarmUpSpellFX.transform.parent = weaponSlotManager.mouthWeaponSlot.transform;
        Destroy(instantiatedWarmUpSpellFX, 3.3f);
    }

    public override void SuccessfullyCastSpell(
        CrocAnimatorManager animatorManager, 
        CrocStatsManager playerStats,
        CrocWeaponSlotManager weaponSlotManager)
    {
        base.SuccessfullyCastSpell(animatorManager, playerStats, weaponSlotManager);
        GameObject instantiatedSpellFX = Instantiate(spellCastFX, weaponSlotManager.mouthWeaponSlot.transform.position, weaponSlotManager.mouthWeaponSlot.transform.rotation);
        rigidBody = instantiatedSpellFX.GetComponent<Rigidbody>();
        //spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();

        rigidBody.AddForce(instantiatedSpellFX.transform.forward * projectileForwardVelocity);
        rigidBody.AddForce(-instantiatedSpellFX.transform.right * projectileUpwardVelocity);
        rigidBody.useGravity = isEffectedByGravity;
        rigidBody.mass = projectileMass;
        instantiatedSpellFX.transform.parent = null;
        Destroy(instantiatedSpellFX, 10f);
    }
}
