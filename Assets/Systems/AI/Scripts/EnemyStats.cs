using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    Animator animator;
    Navigation_CustomPathfinding nav;
    EnemyLocomotionManager enemyLocomotionManager;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        nav = GetComponentInChildren<Navigation_CustomPathfinding>();
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;

        animator.Play("Damage_01");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animator.Play("Dead_01");
            nav.enabled = false;
            enemyLocomotionManager.enabled = false;
        }
    }
}
