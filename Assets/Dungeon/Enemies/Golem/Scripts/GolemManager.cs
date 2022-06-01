using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolemManager : CharacterManager
{
    GolemLocomotionManager enemyLocomotionManager;
    GolemAnimatorManager enemyAnimatorManager;
    GolemStatsManager golemStatsManager;
    GolemEffectsManager golemEffectsManager;

    public GolemState currentState;
    public CharacterStatsManager currentTarget;
    public NavMeshAgent navmeshAgent;
    public Rigidbody enemyRigidBody;

    public bool isPerformingAction;
    public float rotationSpeed = 15;
    public float maximumAggroRadius;

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
        golemStatsManager = GetComponent<GolemStatsManager>();
        golemEffectsManager = GetComponentInChildren<GolemEffectsManager>();
        enemyRigidBody = GetComponent<Rigidbody>();
        navmeshAgent = GetComponentInChildren<NavMeshAgent>();
    }

    private void Update()
    {
        HandleRecoveryTimer();
        HandleStateMachine();

        isInteracting = enemyAnimatorManager.animator.GetBool("isInteracting");
        canDoCombo = enemyAnimatorManager.animator.GetBool("canDoCombo");
        canRotate = enemyAnimatorManager.animator.GetBool("canRotate");
        enemyAnimatorManager.animator.SetBool("isDead", golemStatsManager.isDead);
    }

    private void FixedUpdate()
    {
        golemEffectsManager.HandleAllBuildUpEffects();
    }

    private void HandleStateMachine()
    {
        if (golemStatsManager.isDead)
            return;

        if (currentState != null)
        {
            GolemState nextState = currentState.Tick(this, golemStatsManager, enemyAnimatorManager);

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
