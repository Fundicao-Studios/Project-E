using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossManager : CharacterManager
{
    public string bossName;

    BossLocomotionManager bossLocomotionManager;
    BossAnimatorManager bossAnimatorManager;
    UIBossHealthBar bossHealthBar;
    BossStatsManager bossStats;
    BossEffectsManager bossEffectsManager;
    BossCombatStanceState bossCombatStanceState;

    public BossState currentState;
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
    public bool isPhaseShifting;
    public float comboLikelyHood;

    [Header("VFX Da Segunda Fase")]
    public GameObject particleFX;

    private void Awake()
    {
        bossHealthBar = FindObjectOfType<UIBossHealthBar>();
        bossLocomotionManager = GetComponent<BossLocomotionManager>();
        bossAnimatorManager = GetComponentInChildren<BossAnimatorManager>();
        bossStats = GetComponent<BossStatsManager>();
        bossEffectsManager = GetComponentInChildren<BossEffectsManager>();
        enemyRigidBody = GetComponent<Rigidbody>();
        navmeshAgent = GetComponentInChildren<NavMeshAgent>();
        bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
    }

    private void Start()
    {
        bossHealthBar.SetBossName(bossName);
        bossHealthBar.SetBossMaxHealth(bossStats.maxHealth);
    }

    private void Update()
    {
        HandleRecoveryTimer();
        HandleStateMachine();

        isInteracting = bossAnimatorManager.animator.GetBool("isInteracting");
        isPhaseShifting = bossAnimatorManager.animator.GetBool("isPhaseShifting");
        isInvulnerable = bossAnimatorManager.animator.GetBool("isInvulnerable");
        canDoCombo = bossAnimatorManager.animator.GetBool("canDoCombo");
        canRotate = bossAnimatorManager.animator.GetBool("canRotate");
        bossAnimatorManager.animator.SetBool("isDead", bossStats.isDead);
    }

    private void FixedUpdate()
    {
        bossEffectsManager.HandleAllBuildUpEffects();
    }

    public void UpdateBossHealthBar(int currentHealth, int maxHealth)
    {
        bossHealthBar.SetBossCurrentHealth(currentHealth);

        if (currentHealth <= maxHealth / 2 && !bossCombatStanceState.hasPhaseShifted)
        {
            bossCombatStanceState.hasPhaseShifted = true;
            ShiftToSecondPhase();
        }
    }

    public void ShiftToSecondPhase()
    {
        bossAnimatorManager.animator.SetBool("isInvulnerable", true);
        bossAnimatorManager.animator.SetBool("isPhaseShifting", true);
        bossAnimatorManager.PlayTargetAnimation("Phase Shift", true);
        bossCombatStanceState.hasPhaseShifted = true;
    }

    private void HandleStateMachine()
    {
        if (bossStats.isDead)
            return;

        if (currentState != null)
        {
            BossState nextState = currentState.Tick(this, bossStats, bossAnimatorManager);

            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    private void SwitchToNextState(BossState golemState)
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
