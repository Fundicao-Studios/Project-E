using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocPursueTargetState : CrocState
{
    public CrocCombatStanceSate golemCombatStanceSate;

    public override CrocState Tick(CrocManager golemManager, CrocStatsManager enemyStats, CrocAnimatorManager enemyAnimatorManager)
    {
        float distanceFromTarget = Vector3.Distance(golemManager.currentTarget.transform.position, golemManager.transform.position);

        golemManager.navmeshAgent.SetDestination(golemManager.currentTarget.transform.position);
        enemyAnimatorManager.animator.SetBool("isWalking", true);
        
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
