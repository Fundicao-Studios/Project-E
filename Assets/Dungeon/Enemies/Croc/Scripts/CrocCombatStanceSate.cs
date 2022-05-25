using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocCombatStanceSate : CrocState
{
    public CrocAttackState golemAttackState;
    public EnemyAttackAction[] enemyAttacks;
    public CrocPursueTargetState golemPursueTargetState;

    public override CrocState Tick(CrocManager golemManager, CrocStats enemyStats, CrocAnimatorManager enemyAnimatorManager)
    {
        float distanceFromTarget = Vector3.Distance(golemManager.currentTarget.transform.position, golemManager.transform.position);
        golemAttackState.hasPerformedAttack = false;

        if (golemManager.isInteracting)
        {
            return this;
        }

        else if (distanceFromTarget > golemManager.maximumAggroRadius)
        {
            enemyAnimatorManager.anim.SetBool("isWalking", true);
            return golemPursueTargetState;
        }

        golemManager.navmeshAgent.SetDestination(golemManager.currentTarget.transform.position);

        if (golemManager.currentRecoveryTime <= 0 && golemAttackState.currentAttack != null)
        {
            return golemAttackState;
        }
        else
        {
            GetNewAttack(golemManager);
        }

        return this;
    }

    private void GetNewAttack(CrocManager golemManager)
    {
        Vector3 targetsDirection = golemManager.currentTarget.transform.position - golemManager.transform.position;
        float viewableAngle = Vector3.Angle(targetsDirection, golemManager.transform.forward);
        float distanceFromTarget = Vector3.Distance(golemManager.currentTarget.transform.position, golemManager.transform.position);

        if (distanceFromTarget <= enemyAttacks[0].maximumDistanceNeededToAttack 
            && distanceFromTarget >= enemyAttacks[0].minimumDistanceNeededToAttack)
        {
            if (viewableAngle <= enemyAttacks[0].maximumAttackAngle
                && viewableAngle >= enemyAttacks[0].minimumAttackAngle)
            {
                golemAttackState.currentAttack = enemyAttacks[0];
            }
        }
        
        if (distanceFromTarget <= enemyAttacks[1].maximumDistanceNeededToAttack 
                 && distanceFromTarget >= enemyAttacks[1].minimumDistanceNeededToAttack)
        {
            if (viewableAngle <= enemyAttacks[1].maximumAttackAngle
                && viewableAngle >= enemyAttacks[1].minimumAttackAngle)
            {
                golemAttackState.currentAttack = enemyAttacks[1];
            }
        }
    }
}
