using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemCombatStanceSate : GolemState
{
    public GolemAttackState golemAttackState;
    public EnemyAttackAction[] enemyAttacks;
    public GolemPursueTargetState golemPursueTargetState;

    public override GolemState Tick(GolemManager golemManager, GolemStats enemyStats, GolemAnimatorManager enemyAnimatorManager)
    {
        float distanceFromTarget = Vector3.Distance(golemManager.currentTarget.transform.position, golemManager.transform.position);
        golemAttackState.hasPerformedAttack = false;

        if (golemManager.isInteracting)
        {
            return this;
        }

        else if (distanceFromTarget > golemManager.maximumAggroRadius)
        {
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

    private void GetNewAttack(GolemManager golemManager)
    {
        Vector3 targetsDirection = golemManager.currentTarget.transform.position - golemManager.transform.position;
        float viewableAngle = Vector3.Angle(targetsDirection, golemManager.transform.forward);
        float distanceFromTarget = Vector3.Distance(golemManager.currentTarget.transform.position, golemManager.transform.position);

        int maxScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack 
                && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    maxScore += enemyAttackAction.attackScore;
                }
            }
        }

        int randomValue = Random.Range(0, maxScore);
        int temporaryScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack 
                && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    if (golemAttackState.currentAttack != null)
                        return;

                    temporaryScore += enemyAttackAction.attackScore;

                    if (temporaryScore > randomValue)
                    {
                        golemAttackState.currentAttack = enemyAttackAction;
                    }
                }
            }
        }
    }
}
