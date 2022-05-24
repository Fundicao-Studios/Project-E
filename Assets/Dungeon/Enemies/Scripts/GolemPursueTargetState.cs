using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemPursueTargetState : GolemState
{
    public GolemCombatStanceSate golemCombatStanceSate;

    public override GolemState Tick(GolemManager golemManager, GolemStats enemyStats, GolemAnimatorManager enemyAnimatorManager)
    {
        float distanceFromTarget = Vector3.Distance(golemManager.currentTarget.transform.position, golemManager.transform.position);

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
