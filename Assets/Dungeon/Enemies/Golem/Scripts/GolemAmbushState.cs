using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAmbushState : GolemState
{
    public float detectionRadius = 10;
    public string sleepAnimation;
    public string wakeAnimation;
    public bool isSleeping;

    public LayerMask detectionLayer;

    public GolemPursueTargetState golemPursueTargetState;

    public override GolemState Tick(GolemManager golemManager, GolemStats enemyStats, GolemAnimatorManager enemyAnimatorManager)
    {
        if (isSleeping && golemManager.isInteracting == false)
        {
            enemyAnimatorManager.PlayTargetAnimation(sleepAnimation, true);
        }

        #region Controlar A Deteção Do Alvo

        Collider[] colliders = Physics.OverlapSphere(golemManager.transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if (characterStats != null)
            {
                Vector3 targetsDirection = characterStats.transform.position - golemManager.transform.position;
                float viewableAngle = Vector3.Angle(targetsDirection, golemManager.transform.forward);

                if (viewableAngle > golemManager.minimumDetectionAngle
                    && viewableAngle < golemManager.maximumDetectionAngle)
                {
                    golemManager.currentTarget = characterStats;
                    enemyAnimatorManager.PlayTargetAnimation(wakeAnimation, true);
                    isSleeping = false;
                }
            }            
        }

        #endregion
    
        #region Controlar A Mudança De Estado
        if (golemManager.currentTarget != null)
        {
            return golemPursueTargetState;
        }
        else
        {
            return this;
        }
        #endregion
    }
}
