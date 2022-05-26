using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : BossState
{
    public BossCombatStanceState golemCombatStanceSate;
    public BossPursueTargetState golemPursueTargetState;
    public EnemyAttackAction currentAttack;

    bool willDoComboOnNextAttack = false;
    public bool hasPerformedAttack = false;

    public override BossState Tick(EnemyBossManager golemManager, BossStats enemyStats, BossAnimatorManager enemyAnimatorManager)
    {
        float distanceFromTarget = Vector3.Distance(golemManager.currentTarget.transform.position, golemManager.transform.position);

        RotateTowardsTargetWhilstAttacking(golemManager);

        if (distanceFromTarget > golemManager.maximumAggroRadius)
        {
            return golemPursueTargetState;
        }

        if (willDoComboOnNextAttack && golemManager.canDoCombo)
        {
            AttackTargetWithCombo(enemyAnimatorManager, golemManager);
        }

        if (!hasPerformedAttack)
        {
            AttackTarget(enemyAnimatorManager, golemManager);
            RollForComboChance(golemManager);
        }

        if (willDoComboOnNextAttack && hasPerformedAttack)
        {
            return this; //VOLTA A FAZER O COMBO
        }

        return golemCombatStanceSate;
    }

    private void AttackTarget(BossAnimatorManager enemyAnimatorManager, EnemyBossManager golemManager)
    {
        enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
        golemManager.currentRecoveryTime = currentAttack.recoveryTime;
        hasPerformedAttack = true;
    }

    private void AttackTargetWithCombo(BossAnimatorManager enemyAnimatorManager, EnemyBossManager golemManager)
    {
        willDoComboOnNextAttack = false;
        enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
        golemManager.currentRecoveryTime = currentAttack.recoveryTime;
        //currentAttack = null;
    }

    private void RotateTowardsTargetWhilstAttacking(EnemyBossManager golemManager)
    {
        //Rotacionar manualmente
        if (golemManager.canRotate && golemManager.isInteracting)
        {
            Vector3 direction = golemManager.currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            golemManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, golemManager.rotationSpeed / Time.deltaTime);
        }
    }

    private void RollForComboChance(EnemyBossManager golemManager)
    {
        float comboChance = Random.Range(0, 100);

        if (golemManager.allowAIToPerformCombos && comboChance <= golemManager.comboLikelyHood)
        {
            if (currentAttack.comboAction != null)
            {
                willDoComboOnNextAttack = true;
                currentAttack = currentAttack.comboAction;
            }
            else
            {
                willDoComboOnNextAttack = false;
                currentAttack = null;
            }
        }
    }
}
