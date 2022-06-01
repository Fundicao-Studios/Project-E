using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrocManager : MonoBehaviour
{
    [Header("Transform Para Lock On")]
    public Transform lockOnTransform;

    [Header("Flags De Movimento")]
    public bool canRotate;

    public int pendingCriticalDamage;

    CrocLocomotionManager crocLocomotionManager;
    CrocAnimatorManager crocAnimatorManager;
    CrocStatsManager crocStatsManager;
    CrocEffectsManager crocEffectsManager;

    public CrocState currentState;
    public CharacterStatsManager currentTarget;
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
        crocLocomotionManager = GetComponent<CrocLocomotionManager>();
        crocAnimatorManager = GetComponentInChildren<CrocAnimatorManager>();
        crocStatsManager = GetComponent<CrocStatsManager>();
        crocEffectsManager = GetComponentInChildren<CrocEffectsManager>();
        enemyRigidBody = GetComponent<Rigidbody>();
        navmeshAgent = GetComponentInChildren<NavMeshAgent>();
    }

    private void Update()
    {
        HandleRecoveryTimer();
        HandleStateMachine();

        isInteracting = crocAnimatorManager.animator.GetBool("isInteracting");
        canDoCombo = crocAnimatorManager.animator.GetBool("canDoCombo");
        canRotate = crocAnimatorManager.animator.GetBool("canRotate");
        crocAnimatorManager.animator.SetBool("isDead", crocStatsManager.isDead);
    }

    private void FixedUpdate()
    {
        crocEffectsManager.HandleAllBuildUpEffects();
    }

    private void HandleStateMachine()
    {
        if (crocStatsManager.isDead)
            return;

        if (currentState != null)
        {
            CrocState nextState = currentState.Tick(this, crocStatsManager, crocAnimatorManager);

            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    private void SwitchToNextState(CrocState golemState)
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
