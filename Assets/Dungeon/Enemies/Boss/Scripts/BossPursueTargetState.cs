using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPursueTargetState : BossState
{
    public BossCombatStanceState golemCombatStanceSate;

    public override BossState Tick(BossManager golemManager, BossStatsManager enemyStats, BossAnimatorManager enemyAnimatorManager)
    {
        float distanceFromTarget = Vector3.Distance(golemManager.currentTarget.transform.position, golemManager.transform.position);

        enemyAnimatorManager.PlayTargetAnimation("WalkLoop", false);
        golemManager.navmeshAgent.SetDestination(golemManager.currentTarget.transform.position);
        
        if (distanceFromTarget <= golemManager.maximumAggroRadius)
        {
            return golemCombatStanceSate;
        }
        else
        {
            return this;
        }
    }
}
