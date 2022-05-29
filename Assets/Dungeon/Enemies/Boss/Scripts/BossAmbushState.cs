using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAmbushState : BossState
{
    public float detectionRadius = 10;
    public string sleepAnimation;
    public string wakeAnimation;
    public bool isSleeping;

    public LayerMask detectionLayer;

    public BossPursueTargetState golemPursueTargetState;

    public override BossState Tick(BossManager golemManager, BossStatsManager enemyStats, BossAnimatorManager enemyAnimatorManager)
    {
        if (isSleeping && golemManager.isInteracting == false)
        {
            enemyAnimatorManager.PlayTargetAnimation(sleepAnimation, true);
        }

        #region Controlar A Deteção Do Alvo

        Collider[] colliders = Physics.OverlapSphere(golemManager.transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStatsManager characterStats = colliders[i].transform.GetComponent<CharacterStatsManager>();

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
