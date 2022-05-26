using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStats : CharacterStats
{
    BossAnimatorManager enemyAnimatorManager;
    EnemyBossManager enemyBossManager;

    private void Awake()
    {
        enemyAnimatorManager = GetComponentInChildren<BossAnimatorManager>();
        enemyBossManager = GetComponent<EnemyBossManager>();
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }

    public override void HandlePoiseResetTimer()
    {
        if (poiseResetTimer > 0)
        {
            poiseResetTimer = poiseResetTimer - Time.deltaTime;
        }
        else if (poiseResetTimer <= 0 && !enemyBossManager.isInteracting)
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

        if (enemyBossManager != null)
        {
            enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    public void BreakGuard()
    {
        if (!isDead)
        {
            enemyAnimatorManager.PlayTargetAnimation("Break Guard", true);
        }
    }

    public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
    {
        base.TakeDamage(damage, damageAnimation = "Damage_01");

        if (enemyBossManager != null)
        {
            enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }
        
        enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

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
