using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    PlayerManager playerManager;

    public HealthBar healthBar;
    public StaminaBar staminaBar;
    public ManaPointsBar manaPointsBar;
    PlayerAnimatorManager animatorManager;

    public float staminaRegenerationAmount = 1;
    public float staminaRegenTimer = 0;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();

        healthBar = FindObjectOfType<HealthBar>();
        staminaBar = FindObjectOfType<StaminaBar>();
        manaPointsBar = FindObjectOfType<ManaPointsBar>();
        animatorManager = GetComponentInChildren<PlayerAnimatorManager>();
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetCurrentHealth(currentHealth);

        maxStamina = SetMaxStaminaFromStaminaLevel();
        currenStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
        staminaBar.SetCurrentStamina(currenStamina);

        maxManaPoints = SetMaxManaPointsFromManaLevel();
        currentManaPoints = maxManaPoints;
        manaPointsBar.SetMaxManaPoints(maxManaPoints);
        manaPointsBar.SetCurrentManaPoints(currentManaPoints);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    private float SetMaxManaPointsFromManaLevel()
    {
        maxManaPoints = manaLevel * 10;
        return maxManaPoints;
    }

    private float SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }

    public void TakeDamage(int damage, string damageAnimation = "Damage_01")
    {
        if (playerManager.isInvulnerable)
            return;

        if (isDead)
            return;

        currentHealth = currentHealth - damage;
        healthBar.SetCurrentHealth(currentHealth);

        animatorManager.PlayTargetAnimation(damageAnimation, true);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animatorManager.PlayTargetAnimation("Dead_01", true);
            isDead = true;
            //HANDLE PLAYER DEATH
        }
    }

    public void TakeDamageNoAnimation(int damage)
    {
        currentHealth = currentHealth - damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public void TakeStaminaDamage(float damage)
    {
        currenStamina = currenStamina - damage;
        staminaBar.SetCurrentStamina(currenStamina);
    }

    public void RegenerateStamina()
    {
        if (playerManager.isInteracting)
        {
            staminaRegenTimer = 0;
        }
        else
        {
            staminaRegenTimer += Time.deltaTime;

            if (currenStamina < maxStamina && staminaRegenTimer > 1f)
            {
                currenStamina += staminaRegenerationAmount * Time.deltaTime;
                staminaBar.SetCurrentStamina(Mathf.RoundToInt(currenStamina));
            }
        }
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth = currentHealth + healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthBar.SetCurrentHealth(currentHealth);
    }

    public void DeductManaPoints(int manaPoints)
    {
        currentManaPoints = currentManaPoints - manaPoints;

        if (currentManaPoints < 0)
        {
            currentManaPoints = 0;
        }

        manaPointsBar.SetCurrentManaPoints(currentManaPoints);
    }
}
