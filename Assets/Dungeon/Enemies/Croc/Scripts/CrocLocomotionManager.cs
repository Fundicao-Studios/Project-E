using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocLocomotionManager : MonoBehaviour
{
    CrocManager enemyManager;
    CrocAnimatorManager enemyAnimatorManager;

    public CapsuleCollider characterCollider;

    public LayerMask detectionLayer;

    private void Awake()
    {
        enemyManager = GetComponent<CrocManager>();
        enemyAnimatorManager = GetComponentInChildren<CrocAnimatorManager>();
    }
}
