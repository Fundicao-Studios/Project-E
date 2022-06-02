using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemStatsManager : CharacterStatsManager
{
    GolemAnimatorManager golemAnimatorManager;
    GolemManager enemyManager;
    public UIEnemyHealthBar enemyHealthBar;

    private void Awake()
    {
        golemAnimatorManager = GetComponentInChildren<GolemAnimatorManager>();
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

    public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage, int shockDamage)
    {
        base.TakeDamageNoAnimation(physicalDamage, fireDamage, shockDamage);
        enemyHealthBar.SetHealth(currentHealth);
    }

    public void BreakGuard()
    {
        golemAnimatorManager.PlayTargetAnimation("Break Guard", true);
    }

    public override void TakeDamage(int physicalDamage, int fireDamage, int shockDamage, string damageAnimation = "Damage_01")
    {
        base.TakeDamage(physicalDamage, fireDamage, shockDamage, damageAnimation = "Damage_01");

        enemyHealthBar.SetHealth(currentHealth);
        
        if (!golemAnimatorManager.isPunch01)
        {
            golemAnimatorManager.PlayTargetAnimation(damageAnimation, true);
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
            golemAnimatorManager.PlayTargetAnimation("Dead_01", true);
        }
    }

    private void HandleDeath()
    {
        currentHealth = 0;
        golemAnimatorManager.PlayTargetAnimation("Dead_01", true);
        isDead = true;
    }
}
