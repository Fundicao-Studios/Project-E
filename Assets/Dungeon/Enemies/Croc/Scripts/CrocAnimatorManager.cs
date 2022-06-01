using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocAnimatorManager : AnimatorHandler
{
    CrocManager crocManager;
    CrocStatsManager crocStatsManager;
    CrocWeaponSlotManager crocWeaponSlotManager;

    public CrocSpellItem spellItem; 

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        crocManager = GetComponentInParent<CrocManager>();
        crocStatsManager = GetComponentInParent<CrocStatsManager>();
        crocWeaponSlotManager = GetComponent<CrocWeaponSlotManager>();
    }

    public override void TakeCriticalDamageAnimationEvent()
    {
        crocStatsManager.TakeDamageNoAnimation(crocManager.pendingCriticalDamage, 0);
        crocManager.pendingCriticalDamage = 0;
    }

    private void AttemptToCastSpell()
    {
        spellItem.AttemptToCastSpell(this, crocStatsManager, crocWeaponSlotManager);
    }

    private void SuccessfullyCastSpell()
    {
        spellItem.SuccessfullyCastSpell(this, crocStatsManager, crocWeaponSlotManager);
    }
}
