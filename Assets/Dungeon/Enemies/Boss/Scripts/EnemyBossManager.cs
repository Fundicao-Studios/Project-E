using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBossManager : CharacterManager
{
    public string bossName;

    BossLocomotionManager enemyLocomotionManager;
    BossAnimatorManager enemyAnimatorManager;
    UIBossHealthBar bossHealthBar;
    BossStats enemyStats;
    BossCombatStanceState bossCombatStanceState;

    public BossState currentState;
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
    public bool isPhaseShifting;
    public float comboLikelyHood;

    [Header("VFX Da Segunda Fase")]
    public GameObject particleFX;

    private void Awake()
    {
        bossHealthBar = FindObjectOfType<UIBossHealthBar>();
        enemyLocomotionManager = GetComponent<BossLocomotionManager>();
        enemyAnimatorManager = GetComponentInChildren<BossAnimatorManager>();
        enemyStats = GetComponent<BossStats>();
        enemyRigidBody = GetComponent<Rigidbody>();
        navmeshAgent = GetComponentInChildren<NavMeshAgent>();
        bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
    }

    private void Start()
    {
        bossHealthBar.SetBossName(bossName);
        bossHealthBar.SetBossMaxHealth(enemyStats.maxHealth);
    }

    private void Update()
    {
        HandleRecoveryTimer();
        HandleStateMachine();

        isInteracting = enemyAnimatorManager.anim.GetBool("isInteracting");
        isPhaseShifting = enemyAnimatorManager.anim.GetBool("isPhaseShifting");
        isInvulnerable = enemyAnimatorManager.anim.GetBool("isInvulnerable");
        canDoCombo = enemyAnimatorManager.anim.GetBool("canDoCombo");
        canRotate = enemyAnimatorManager.anim.GetBool("canRotate");
        enemyAnimatorManager.anim.SetBool("isDead", enemyStats.isDead);
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
        enemyAnimatorManager.anim.SetBool("isInvulnerable", true);
        enemyAnimatorManager.anim.SetBool("isPhaseShifting", true);
        enemyAnimatorManager.PlayTargetAnimation("Phase Shift", true);
        bossCombatStanceState.hasPhaseShifted = true;
    }

    private void HandleStateMachine()
    {
        if (enemyStats.isDead)
            return;

        if (currentState != null)
        {
            BossState nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

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
