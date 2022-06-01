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

    public override GolemState Tick(GolemManager golemManager, GolemStatsManager golemStatsManager, GolemAnimatorManager golemAnimatorManager)
    {
        if (isSleeping && golemManager.isInteracting == false)
        {
            golemAnimatorManager.PlayTargetAnimation(sleepAnimation, true);
        }

        #region Controlar A Deteção Do Alvo

        Collider[] colliders = Physics.OverlapSphere(golemManager.transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStatsManager characterStats = colliders[i].transform.GetComponent<CharacterStatsManager>();

            if (characterStats != null)
            {
                if (characterStats.teamIDNumber == golemStatsManager.teamIDNumber)
                    return this;

                Vector3 targetsDirection = characterStats.transform.position - golemManager.transform.position;
                float viewableAngle = Vector3.Angle(targetsDirection, golemManager.transform.forward);

                if (viewableAngle > golemManager.minimumDetectionAngle
                    && viewableAngle < golemManager.maximumDetectionAngle)
                {
                    golemManager.currentTarget = characterStats;
                    golemAnimatorManager.PlayTargetAnimation(wakeAnimation, true);
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
