using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemIdleState : GolemState
{
    public GolemPursueTargetState golemPursueTargetState;
    public LayerMask detectionLayer;

    public override GolemState Tick(GolemManager golemManager, GolemStatsManager enemyStats, GolemAnimatorManager enemyAnimatorManager)
    {
        #region Controlar A Deteção De Alvos Do Inimigo
        Collider[] colliders = Physics.OverlapSphere(transform.position, golemManager.detectionRadius, detectionLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStatsManager characterStats = colliders[i].transform.GetComponent<CharacterStatsManager>();

            if (characterStats != null)
            {
                //VERIFICAR PELO ID DE EQUIPA

                Vector3 targetDirection = characterStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > golemManager.minimumDetectionAngle && viewableAngle < golemManager.maximumDetectionAngle)
                {
                    golemManager.currentTarget = characterStats;
                }
            }
        }
        #endregion

        #region Controlar A Mudança Para O Próximo Estado
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
