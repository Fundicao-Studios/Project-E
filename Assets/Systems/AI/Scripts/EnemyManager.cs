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
    public float maximumAggroRadius = 1.5f;

    [Header("Combat Flags")]
    public bool canDoCombo;

    [Header("Definições do AI")]
    public float detectionRadius = 20;
    //Quanto maior, e menor, respetivamente forem estes ângulos, maior a deteção CAMPO DE VISÃO (basicamente visão periférica)
    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;
    public float viewableAngle;
    public float detectionDistance;
    public float currentRecoveryTime = 5;

    [Header("Definições De Combate Do AI")]
    public bool allowAIToPerformCombos;
    public float comboLikelyHood;

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
        HandleStateMachine();

        isInteracting = enemyAnimatorManager.anim.GetBool("isInteracting");
        canDoCombo = enemyAnimatorManager.anim.GetBool("canDoCombo");
        canRotate = enemyAnimatorManager.anim.GetBool("canRotate");
        enemyAnimatorManager.anim.SetBool("isDead", enemyStats.isDead);
    }

    private void LateUpdate()
    {
        navmeshAgent.transform.localPosition = Vector3.zero;
        navmeshAgent.transform.localRotation = Quaternion.identity;
    }

    private void HandleStateMachine()
    {
        if (enemyStats.isDead)
            return;

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
