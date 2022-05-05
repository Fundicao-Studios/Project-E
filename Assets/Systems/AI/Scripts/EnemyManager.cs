using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
    EnemyLocomotionManager enemyLocomotionManager;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyStats enemyStats;

    public State currentState;
    public CharacterStats currentTarget;
    public Navigation_CustomPathfinding navmeshAgent;
    public Rigidbody enemyRigidBody;

    public bool isPerformingAction;
    public bool isInteracting;
    public float rotationSpeed = 15;
    public float maximumAttackRange = 1.5f;

    [Header("Definições do AI")]
    public float detectionRadius = 20;
    //Quanto maior, e menor, respetivamente forem estes ângulos, maior a deteção CAMPO DE VISÃO (basicamente visão periférica)
    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;
    public float viewableAngle;
    public float detectionDistance;

    public float currentRecoveryTime = 5;

    private void Awake()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        enemyStats = GetComponent<EnemyStats>();
        enemyRigidBody = GetComponent<Rigidbody>();
        navmeshAgent = GetComponentInChildren<Navigation_CustomPathfinding>();
        navmeshAgent.enabled = false;
    }

    private void Start()
    {
        enemyRigidBody.isKinematic = false;
    }

    private void Update()
    {
        HandleRecoveryTimer();

        isInteracting = enemyAnimatorManager.anim.GetBool("isInteracting");
    }

    private void FixedUpdate()
    {
        if (enemyStats.isDead)
            return;

        HandleStateMachine();
    }

    private void HandleStateMachine()
    {
        if (currentState != null)
        {
            State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    private void SwitchToNextState(State state)
    {
        currentState = state;
    }

    private void HandleRecoveryTimer()
    {
        if (currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if (isPerformingAction)
        {
            if (currentRecoveryTime <= 0)
            {
                isPerformingAction = false;
            }
        }
    }
}
