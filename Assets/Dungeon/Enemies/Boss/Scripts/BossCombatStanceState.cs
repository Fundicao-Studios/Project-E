using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombatStanceState : BossState
{
    public BossAttackState golemAttackState;
    public EnemyAttackAction[] enemyAttacks;
    public BossPursueTargetState golemPursueTargetState;

    [Header("Ataques Da Segunda Fase")]
    public bool hasPhaseShifted;
    public EnemyAttackAction[] secondPhaseAttacks;

    public override BossState Tick(BossManager golemManager, BossStatsManager enemyStats, BossAnimatorManager enemyAnimatorManager)
    {
        float distanceFromTarget = Vector3.Distance(golemManager.currentTarget.transform.position, golemManager.transform.position);
        golemAttackState.hasPerformedAttack = false;

        if (golemManager.isInteracting)
        {
            return this;
        }

        else if (distanceFromTarget > golemManager.maximumAggroRadius)
        {
            enemyAnimatorManager.animator.SetBool("isWalking", true);
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

    private void GetNewAttack(BossManager enemyManager)
    {
        if (!hasPhaseShifted)
        {
            Vector3 targetsDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, enemyManager.transform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

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

        if(hasPhaseShifted)
        {
            Vector3 targetsDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, enemyManager.transform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            int maxScore = 0;

            for (int i = 0; i < secondPhaseAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = secondPhaseAttacks[i];

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

            for (int i = 0; i < secondPhaseAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = secondPhaseAttacks[i];

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
}
