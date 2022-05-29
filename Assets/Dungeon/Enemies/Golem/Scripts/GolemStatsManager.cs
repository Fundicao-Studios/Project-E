using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemStatsManager : CharacterStatsManager
{
    GolemAnimatorManager enemyAnimatorManager;
    GolemManager enemyManager;
    public UIEnemyHealthBar enemyHealthBar;

    private void Awake()
    {
        enemyAnimatorManager = GetComponentInChildren<GolemAnimatorManager>();
        enemyManager = GetComponent<GolemManager>();
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }

    void Start()
    {
        enemyHealthBar.SetMaxHealth(maxHealth);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public override void TakeDamageNoAnimation(int damage)
    {
        base.TakeDamageNoAnimation(damage);
        enemyHealthBar.SetHealth(currentHealth);
    }

    public void BreakGuard()
    {
        enemyAnimatorManager.PlayTargetAnimation("Break Guard", true);
    }

    public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
    {
        base.TakeDamage(damage, damageAnimation = "Damage_01");

        enemyHealthBar.SetHealth(currentHealth);
        
        if (!enemyAnimatorManager.isPunch01)
        {
            enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);
        }

        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        currentHealth = 0;
        enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
        isDead = true;
    }
}
