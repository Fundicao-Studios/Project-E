using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimatorManager : AnimatorHandler
{
    EnemyBossManager enemyBossManager;
    BossStats enemyStats;

    public bool isPunch01;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyBossManager = GetComponentInParent<EnemyBossManager>();
        enemyStats = GetComponentInParent<BossStats>();
    }

    public override void TakeCriticalDamageAnimationEvent()
    {
        enemyStats.TakeDamageNoAnimation(enemyBossManager.pendingCriticalDamage);
        enemyBossManager.pendingCriticalDamage = 0;
    }

    public void EnableIsParrying()
    {
        enemyBossManager.isParrying = true;
    }

    public void DisableIsParrying()
    {
        enemyBossManager.isParrying = false;
    }

    public void EnableCanBeRiposted()
    {
        enemyBossManager.canBeRiposted = true;
    }

    public void DisableCanBeRiposted()
    {
        enemyBossManager.canBeRiposted = false;
    }

    public void EnablePunch01()
    {
        isPunch01 = true;
    }

    public void DisablePunch01()
    {
        isPunch01 = false;
    }

    public void InstantiateBossParticleFX()
    {
        BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();
        GameObject phaseFX = Instantiate(enemyBossManager.particleFX, bossFXTransform.transform);
    }
}
