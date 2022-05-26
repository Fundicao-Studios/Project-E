using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemStats : CharacterStats
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

    public override void HandlePoiseResetTimer()
    {
        if (poiseResetTimer > 0)
        {
            poiseResetTimer = poiseResetTimer - Time.deltaTime;
        }
        else if (poiseResetTimer <= 0 && !enemyManager.isInteracting)
        {
            totalPoiseDefense = armorPoiseBonus;
        }
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
