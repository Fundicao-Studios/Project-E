using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocAnimatorManager : AnimatorHandler
{
    CrocManager golemManager;
    CrocStats enemyStats;
    CrocWeaponSlotManager weaponSlotManager;

    public CrocSpellItem spellItem; 

    private void Awake()
    {
        anim = GetComponent<Animator>();
        golemManager = GetComponentInParent<CrocManager>();
        enemyStats = GetComponentInParent<CrocStats>();
        weaponSlotManager = GetComponent<CrocWeaponSlotManager>();
    }

    public override void TakeCriticalDamageAnimationEvent()
    {
        enemyStats.TakeDamageNoAnimation(golemManager.pendingCriticalDamage);
        golemManager.pendingCriticalDamage = 0;
    }

    private void AttemptToCastSpell()
    {
        spellItem.AttemptToCastSpell(this, enemyStats, weaponSlotManager);
    }

    private void SuccessfullyCastSpell()
    {
        spellItem.SuccessfullyCastSpell(this, enemyStats, weaponSlotManager);
    }
}
