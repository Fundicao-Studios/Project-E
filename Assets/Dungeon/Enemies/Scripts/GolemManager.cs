using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolemManager : CharacterManager
{
    GolemLocomotionManager enemyLocomotionManager;
    GolemAnimatorManager enemyAnimatorManager;
    GolemStats enemyStats;

    public GolemState currentState;
    public CharacterStats currentTarget;
    public NavMeshAgent navmeshAgent;
    public Rigidbody enemyRigidBody;

    public bool isPerformingAction;
    public bool isInteracting;
    public float rotationSpeed = 15;
    public float maximumAggroRadius;

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
        enemyLocomotionManager = GetComponent<GolemLocomotionManager>();
        enemyAnimatorManager = GetComponentInChildren<GolemAnimatorManager>();
        enemyStats = GetComponent<GolemStats>();
        enemyRigidBody = GetComponent<Rigidbody>();
        navmeshAgent = GetComponentInChildren<NavMeshAgent>();
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

    private void HandleStateMachine()
    {
        if (enemyStats.isDead)
            return;

        if (currentState != null)
        {
            GolemState nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    private void SwitchToNextState(GolemState golemState)
    {
        currentState = golemState;
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
