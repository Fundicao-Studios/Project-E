using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLocomotionManager : MonoBehaviour
{
    BossManager enemyManager;
    BossAnimatorManager enemyAnimatorManager;

    public CapsuleCollider characterCollider;
    public CapsuleCollider characterCollisionBlockerCollider;

    public LayerMask detectionLayer;

    private void Awake()
    {
        enemyManager = GetComponent<BossManager>();
        enemyAnimatorManager = GetComponentInChildren<BossAnimatorManager>();
    }

    private void Start()
    {
        Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
    }
}
