using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    Animator animator;
    Navigation_CustomPathfinding nav;
    EnemyLocomotionManager enemyLocomotionManager;

    public UIEnemyHealthBar enemyHealthBar;

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
        enemyHealthBar.SetMaxHealth(maxHealth);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamageNoAnimation(int damage)
    {
        currentHealth = currentHealth - damage;
        enemyHealthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
    {
        if (isDead)
            return;
            
        currentHealth = currentHealth - damage;
        enemyHealthBar.SetHealth(currentHealth);

        animator.Play("Damage_01");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animator.Play("Dead_01");
            isDead = true;
        }
    }
}
