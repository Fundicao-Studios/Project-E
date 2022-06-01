using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocIdleState : CrocState
{
    public CrocPursueTargetState golemPursueTargetState;
    public LayerMask detectionLayer;

    public override CrocState Tick(CrocManager crocManager, CrocStatsManager crocStatsManager, CrocAnimatorManager crocAnimatorManager)
    {
        #region Controlar A Deteção De Alvos Do Inimigo
        Collider[] colliders = Physics.OverlapSphere(transform.position, crocManager.detectionRadius, detectionLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStatsManager characterStats = colliders[i].transform.GetComponent<CharacterStatsManager>();

            if (characterStats != null)
            {
                if (characterStats.teamIDNumber == crocStatsManager.teamIDNumber)
                    return this;

                Vector3 targetDirection = characterStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > crocManager.minimumDetectionAngle && viewableAngle < crocManager.maximumDetectionAngle)
                {
                    crocManager.currentTarget = characterStats;
                }
            }
        }
        #endregion

        #region Controlar A Mudança Para O Próximo Estado
        if (crocManager.currentTarget != null)
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
