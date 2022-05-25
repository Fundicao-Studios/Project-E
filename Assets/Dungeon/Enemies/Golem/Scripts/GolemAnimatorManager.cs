using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAnimatorManager : AnimatorHandler
{
    GolemManager golemManager;
    GolemStats enemyStats;

    public bool isPunch01;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        golemManager = GetComponentInParent<GolemManager>();
        enemyStats = GetComponentInParent<GolemStats>();
    }

    public override void TakeCriticalDamageAnimationEvent()
    {
        enemyStats.TakeDamageNoAnimation(golemManager.pendingCriticalDamage);
        golemManager.pendingCriticalDamage = 0;
    }

    public void EnableIsParrying()
    {
        golemManager.isParrying = true;
    }

    public void DisableIsParrying()
    {
        golemManager.isParrying = false;
    }

    public void EnableCanBeRiposted()
    {
        golemManager.canBeRiposted = true;
    }

    public void DisableCanBeRiposted()
    {
        golemManager.canBeRiposted = false;
    }

    public void EnablePunch01()
    {
        isPunch01 = true;
    }

    public void DisablePunch01()
    {
        isPunch01 = false;
    }
}
