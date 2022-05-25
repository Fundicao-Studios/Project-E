using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemLocomotionManager : MonoBehaviour
{
    GolemManager enemyManager;
    GolemAnimatorManager enemyAnimatorManager;

    public CapsuleCollider characterCollider;
    public CapsuleCollider characterCollisionBlockerCollider;

    public LayerMask detectionLayer;

    private void Awake()
    {
        enemyManager = GetComponent<GolemManager>();
        enemyAnimatorManager = GetComponentInChildren<GolemAnimatorManager>();
    }

    private void Start()
    {
        Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
    }
}
