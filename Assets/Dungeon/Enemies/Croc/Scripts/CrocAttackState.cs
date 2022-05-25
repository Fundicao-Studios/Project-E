using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocAttackState : CrocState
{
    public CrocCombatStanceSate golemCombatStanceSate;
    public CrocPursueTargetState golemPursueTargetState;
    public EnemyAttackAction currentAttack;

    bool willDoComboOnNextAttack = false;
    public bool hasPerformedAttack = false;

    public override CrocState Tick(CrocManager golemManager, CrocStats enemyStats, CrocAnimatorManager enemyAnimatorManager)
    {
        float distanceFromTarget = Vector3.Distance(golemManager.currentTarget.transform.position, golemManager.transform.position);

        RotateTowardsTargetWhilstAttacking(golemManager);

        if (distanceFromTarget > golemManager.maximumAggroRadius)
        {
            enemyAnimatorManager.anim.SetBool("isWalking", true);
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

    private void AttackTarget(CrocAnimatorManager enemyAnimatorManager, CrocManager golemManager)
    {
        enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, false, true);
        golemManager.currentRecoveryTime = currentAttack.recoveryTime;
        hasPerformedAttack = true;
        currentAttack = null;
    }

    private void AttackTargetWithCombo(CrocAnimatorManager enemyAnimatorManager, CrocManager golemManager)
    {
        willDoComboOnNextAttack = false;
        enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, false, true);
        golemManager.currentRecoveryTime = currentAttack.recoveryTime;
        currentAttack = null;
    }

    private void RotateTowardsTargetWhilstAttacking(CrocManager golemManager)
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

    private void RollForComboChance(CrocManager golemManager)
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
