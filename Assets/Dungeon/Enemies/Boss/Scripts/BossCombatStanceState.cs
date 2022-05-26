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

    public override BossState Tick(EnemyBossManager golemManager, BossStats enemyStats, BossAnimatorManager enemyAnimatorManager)
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

    private void GetNewAttack(EnemyBossManager enemyManager)
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
        else
        {
            Vector3 targetsDirection2 = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float viewableAngle2 = Vector3.Angle(targetsDirection2, enemyManager.transform.forward);
            float distanceFromTarget2 = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            int maxScore2 = 0;

            for (int i = 0; i < secondPhaseAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = secondPhaseAttacks[i];

                if (distanceFromTarget2 <= enemyAttackAction.maximumDistanceNeededToAttack 
                    && distanceFromTarget2 >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle2 <= enemyAttackAction.maximumAttackAngle
                        && viewableAngle2 >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore2 += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue2 = Random.Range(0, maxScore2);
            int temporaryScore2 = 0;

            for (int i = 0; i < secondPhaseAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = secondPhaseAttacks[i];

                if (distanceFromTarget2 <= enemyAttackAction.maximumDistanceNeededToAttack 
                    && distanceFromTarget2 >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle2 <= enemyAttackAction.maximumAttackAngle
                        && viewableAngle2 >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (golemAttackState.currentAttack != null)
                            return;

                        temporaryScore2 += enemyAttackAction.attackScore;

                        if (temporaryScore2 > randomValue2)
                        {
                            golemAttackState.currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }
    }
}
