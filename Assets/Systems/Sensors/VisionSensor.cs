using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class VisionSensor : MonoBehaviour
{
    [SerializeField] LayerMask DetectionMask = ~0;

    EnemyAI LinkedAI;

    // Start is called before the first frame update
    void Start()
    {
        LinkedAI = GetComponent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int index = 0; index < DetectableTargetManager.Instance.AllTargets.Count; ++index)
        {
            var candidateTarget = DetectableTargetManager.Instance.AllTargets[index];

            // passar à frente se o candidato for ele próprio
            if (candidateTarget.gameObject == gameObject)
                continue;

            var vectorToTarget = candidateTarget.transform.position - LinkedAI.EyeLocation;

            // se estiver fora do alcance - não consegue ver
            if (vectorToTarget.sqrMagnitude > (LinkedAI.VisionConeRange * LinkedAI.VisionConeRange))
                continue;

            vectorToTarget.Normalize();

            // se estiver fora do cone de visão - não consegue ver
            if (Vector3.Dot(vectorToTarget, LinkedAI.EyeDirection) < LinkedAI.CosVisionConeAngle)
                continue;

            // o raycast para o alvo passa?
            RaycastHit hitResult;
            if (Physics.Raycast(LinkedAI.EyeLocation, vectorToTarget, out hitResult,
                                LinkedAI.VisionConeRange, DetectionMask, QueryTriggerInteraction.Collide))
            {
                if (hitResult.collider.GetComponentInParent<DetectableTarget>() == candidateTarget)
                    LinkedAI.ReportCanSee(candidateTarget);
            }
        }
    }
}
