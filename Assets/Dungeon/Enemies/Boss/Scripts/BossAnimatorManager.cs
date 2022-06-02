using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimatorManager : CharacterAnimatorManager
{
    BossManager bossManager;
    BossStatsManager bossStats;

    public bool isPunch01;

    protected override void Awake()
    {
        base.Awake();
        bossStats = GetComponentInParent<BossStatsManager>();
        animator = GetComponent<Animator>();
        bossManager = GetComponentInParent<BossManager>();
    }

    public override void TakeCriticalDamageAnimationEvent()
    {
        bossStats.TakeDamageNoAnimation(bossManager.pendingCriticalDamage, 0, 0);
        bossManager.pendingCriticalDamage = 0;
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
        GameObject phaseFX = Instantiate(bossManager.particleFX, bossFXTransform.transform);
    }
}
