using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatsManager : CharacterStatsManager
{
    BossAnimatorManager bossAnimatorManager;
    BossManager bossManager;

    private void Awake()
    {
        bossAnimatorManager = GetComponentInChildren<BossAnimatorManager>();
        bossManager = GetComponent<BossManager>();
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage, int shockDamage)
    {
        base.TakeDamageNoAnimation(physicalDamage, fireDamage, shockDamage);
        if (bossManager != null)
        {
            bossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }

        if (currentHealth == 0)
        {
            HandleDeath();
        }
    }

    public void BreakGuard()
    {
        if (!isDead)
        {
            bossAnimatorManager.PlayTargetAnimation("Break Guard", true);
        }
    }

    public override void TakeDamage(int physicalDamage, int fireDamage, int shockDamage, string damageAnimation = "Damage_01")
    {
        base.TakeDamage(physicalDamage, fireDamage, shockDamage, damageAnimation = "Damage_01");

        if (bossManager != null)
        {
            bossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }
        
        bossAnimatorManager.PlayTargetAnimation(damageAnimation, true);

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

        if (bossManager != null)
        {
            bossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            bossAnimatorManager.PlayTargetAnimation("Dead_01", true);
        }
    }

    private void HandleDeath()
    {
        currentHealth = 0;
        bossAnimatorManager.PlayTargetAnimation("Dead_01", true);
        isDead = true;
    }
}
