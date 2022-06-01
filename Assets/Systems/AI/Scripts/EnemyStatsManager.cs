using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsManager : CharacterStatsManager
{
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyManager enemyManager;
    public UIEnemyHealthBar enemyHealthBar;

    private void Awake()
    {
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        enemyManager = GetComponent<EnemyManager>();
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

    public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
    {
        base.TakeDamageNoAnimation(physicalDamage, fireDamage);
        enemyHealthBar.SetHealth(currentHealth);
    }

    public void BreakGuard()
    {
        enemyAnimatorManager.PlayTargetAnimation("Break Guard", true);
    }

    public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation = "Damage_01")
    {
        base.TakeDamage(physicalDamage, fireDamage, damageAnimation = "Damage_01");
        
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

    public override void TakeBurnDamage(int damage)
    {
        if (isDead)
            return;

        base.TakeBurnDamage(damage);
        enemyHealthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
        }
    }

    private void HandleDeath()
    {
        currentHealth = 0;
        enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
        isDead = true;
    }
}
