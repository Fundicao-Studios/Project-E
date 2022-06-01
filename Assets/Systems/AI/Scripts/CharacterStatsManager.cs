using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    [Header("ID De Equipa")]
    public int teamIDNumber = 0;

    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public int staminaLevel = 10;
    public float maxStamina;
    public float currenStamina;

    public int manaLevel = 10;
    public float maxManaPoints;
    public float currentManaPoints;

    [Header("Poise")]
    public float totalPoiseDefense; //O TOTAL de poise durante o cálculo
    public float offensivePoiseBonus; //O poise que se GANHA durante um ataque com uma arma
    public float armorPoiseBonus; //O poise que se GANHA por estar a usar o que quer que seja que se tenha equipado
    public float totalPoiseResetTime = 15;
    public float poiseResetTimer = 0;

    [Header("Absorção De Armadura")]
    public float physicalDamageAbsorptionHead;
    public float physicalDamageAbsorptionBody;
    public float physicalDamageAbsorptionLegs;
    public float physicalDamageAbsorptionFeet;

    public float fireDamageAbsorptionHead;
    public float fireDamageAbsorptionBody;
    public float fireDamageAbsorptionLegs;
    public float fireDamageAbsorptionFeet;

    //Absorção De Fogo
    //Absorção De Choque
    //Absorção De Água
    //Absorção De Escuridão

    public bool isDead;

    protected virtual void Update()
    {
        HandlePoiseResetTimer();
    }

    private void Start()
    {
        totalPoiseDefense = armorPoiseBonus;
    }

    public virtual void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation = "Damage_01")
    {
        if (isDead)
            return;

        float totalPhysicalDamageAbsorption = 1 - 
            (1 - physicalDamageAbsorptionHead / 100) * 
            (1 - physicalDamageAbsorptionBody / 100) * 
            (1 - physicalDamageAbsorptionLegs / 100) *
            (1 - physicalDamageAbsorptionFeet / 100);

        physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

        float totalFireDamageAbsorption = 1 -
            (1 - fireDamageAbsorptionHead / 100) *
            (1 - fireDamageAbsorptionBody / 100) *
            (1 - fireDamageAbsorptionLegs / 100) *
            (1 - fireDamageAbsorptionFeet / 100);

        fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

        float finalDamage = physicalDamage + fireDamage; //+ darkDamage + lightningDamage + waterDamage

        currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public virtual void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
    {
        if (isDead)
            return;

        float totalPhysicalDamageAbsorption = 1 - 
            (1 - physicalDamageAbsorptionHead / 100) * 
            (1 - physicalDamageAbsorptionBody / 100) * 
            (1 - physicalDamageAbsorptionLegs / 100) *
            (1 - physicalDamageAbsorptionFeet / 100);

        physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

        float totalFireDamageAbsorption = 1 -
            (1 - fireDamageAbsorptionHead / 100) *
            (1 - fireDamageAbsorptionBody / 100) *
            (1 - fireDamageAbsorptionLegs / 100) *
            (1 - fireDamageAbsorptionFeet / 100);

        fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage + totalFireDamageAbsorption));

        float finalDamage = physicalDamage + fireDamage; //+ darkDamage + lightningDamage + waterDamage

        currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public virtual void TakeBurnDamage(int damage)
    {
        currentHealth = currentHealth - damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public virtual void HandlePoiseResetTimer()
    {
        if (poiseResetTimer > 0)
        {
            poiseResetTimer = poiseResetTimer - Time.deltaTime;
        }
        else
        {
            totalPoiseDefense = armorPoiseBonus;
        }
    }
}
